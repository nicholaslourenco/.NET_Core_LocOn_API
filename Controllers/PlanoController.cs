using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LocOn.Models;
using LocOn.Services;
using Microsoft.AspNetCore.Mvc;

namespace LocOn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanoController : BaseApiController
    {
        private readonly PlanoService _planoService;

        public PlanoController(UsuarioService usuarioService, PlanoService planoService) : base(usuarioService)
        {
            _planoService = planoService;
        }

        [HttpPost]
        public IActionResult Registrar(Plano plano)
        {
            var currentUserId = GetCurrentUserId();
            var currentUserType = GetCurrentUserType();

            if (currentUserId == null)
            {
                return BadRequest(new { message = "Você precisa estar logado..." });
            }

            bool isAdmin = currentUserType == "Admin";

            if (!isAdmin)
            {
                return StatusCode(403, new { message = "Você não tem permissão para cadastrar planos." });
            }

            try
            {
                _planoService.Inserir(plano);

                return Ok("Plano cadastrado com sucesso!");
            }
            catch (System.Exception e)
            {
                return BadRequest(new { message = "Erro ao cadastrar plano.", error = e.Message });
            }
        }

        [HttpGet]
        public IActionResult Listar()
        {
            var currentUserId = GetCurrentUserId();

            if (currentUserId == null)
            {
                return BadRequest(new { message = "Você precisa estar logado..." });
            }

            return Ok(_planoService.Listar());
        }

        [HttpGet("id")]
        public IActionResult BuscaId([FromQuery(Name = "id")] int id)
        {
            var currentUserId = GetCurrentUserId();

            if (currentUserId == null)
            {
                return BadRequest(new { message = "Você precisa estar logado..." });
            }

            return Ok(_planoService.BuscaId(id));
        }

        [HttpPut("{id}")]
        public IActionResult Editar(int id, [FromBody] Plano planoEditado)
        {

            var currentUserId = GetCurrentUserId();
            var currentUserType = GetCurrentUserType();

            if (currentUserId == null)
            {
                return Unauthorized(new { message = "Você precisa estar logado..." });
            }

            bool isAdmin = currentUserType == "Admin";

            if (!isAdmin)
            {
                return StatusCode(403, new { message = "Você não tem permissão para editar planos." });
            }

            _planoService.Editar(id, planoEditado);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Excluir(int id)
        {
            var currentUserId = GetCurrentUserId();
            var currentUserType = GetCurrentUserType();

            if (currentUserId == null)
            {
                return Unauthorized(new { message = "Você precisa estar logado..." });
            }

            bool isAdmin = currentUserType == "Admin";

            if (!isAdmin)
            {
                return StatusCode(403, new { message = "Você não tem permissão para excluir planos." });
            }

            _planoService.Excluir(id);
            return NoContent();
        }
    }
}