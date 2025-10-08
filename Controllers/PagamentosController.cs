using Microsoft.AspNetCore.Mvc;
using LocOn.Services;
using LocOn.Models;
using Stripe.Checkout;
using Stripe;

namespace LocOn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagamentosController : BaseApiController
    {
        private readonly StripePaymentService _paymentService;
        private readonly PlanoService _planoService;

        // Injeção de dependência do serviço
        public PagamentosController(UsuarioService usuarioService,
                                    StripePaymentService paymentService,
                                    PlanoService planoService) : base(usuarioService)
        {
            _paymentService = paymentService;
            _planoService = planoService;
        }

        // Endpoint para iniciar o checkout
        [HttpPost("checkout/{planoId}")]
        public IActionResult CreateCheckoutSession(int planoId)
        {
            // 1. OBTEM ID REAL DO USUÁRIO LOGADO
            var currentUserId = GetCurrentUserId();
            if (currentUserId == null)
            {
                return Unauthorized(new { message = "Você precisa estar logado para iniciar um pagamento." });
            }

            // 2. BUSCA O PLANO REAL NO BANCO DE DADOS
            Plano planoReal = _planoService.BuscaId(planoId);
            if (planoReal == null)
            {
                return NotFound(new { message = "Plano não encontrado." });
            }

            // 3. Criar a sessão do Stripe
            try
            {
                // Passamos o objeto Plano real e o ID real
                Session session = _paymentService.CreateCheckoutSession(planoReal, currentUserId.Value);
                
                return Ok(new { url = session.Url });
            }
            catch (StripeException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        // Endpoint para tratar o sucesso do pagamento (simulação de baixa)
        [HttpGet("success")]
        public IActionResult PaymentSuccess([FromQuery(Name = "session_id")] string sessionId)
        {
            try
            {
                // 3. Recupera a sessão para verificar se foi paga
                var session = _paymentService.RetrieveSession(sessionId);

                if (session.PaymentStatus == "paid")
                {
                    // 4. LÓGICA DE BAIXA REAL NO DB
                    
                    // Os metadados devem vir como strings, tentamos converter para int
                    if (!int.TryParse(session.Metadata["user_id"], out int userId) || 
                        !int.TryParse(session.Metadata["plano_id"], out int planoId))
                    {
                        return BadRequest(new { message = "Metadados de usuário ou plano inválidos na sessão Stripe." });
                    }
                    
                    // Buscamos o usuário no DB (usando o _usuarioService da Base)
                    Usuario usuario = _usuarioService.BuscaId(userId);

                    if (usuario == null)
                    {
                        // IMPORTANTE: Se o usuário não existir, é um erro interno.
                        return StatusCode(500, new { message = "Erro interno: Usuário encontrado no Stripe, mas não no DB." });
                    }
                    
                    // Atualizamos o PlanoId e o Tipo
                    usuario.PlanoId = planoId;

                    // Salvamos a mudança no DB (Método Editar do Service atualiza o objeto rastreado)
                    _usuarioService.Editar(userId, usuario);
                    
                    // Retorna sucesso para o usuário.
                    return Ok($"Assinatura ativada com sucesso! Usuário: {userId}, Plano: {planoId}");
                }

                return BadRequest(new { message = "Pagamento não concluído ou pendente." });
            }
            catch (StripeException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }
    }
}