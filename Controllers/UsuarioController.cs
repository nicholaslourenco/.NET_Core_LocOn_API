using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LocOn.DTOs;
using LocOn.Models;
using LocOn.Services;
using BCrypt.Net;

namespace LocOn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : BaseApiController
    {

        private readonly TokenService _tokenService;

        public UsuarioController(UsuarioService usuarioService, TokenService tokenService) : base(usuarioService)
        {
            _tokenService = tokenService;
        }


        [HttpPost("register")]
        public IActionResult Register([FromBody] CadastroDTO dadosCadastro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string senhaHashGerada = BCrypt.Net.BCrypt.HashPassword(dadosCadastro.Senha);

            var novoUsuario = new Usuario
            {
                Login = dadosCadastro.Login,
                Nome = dadosCadastro.Nome,
                SenhaHash = senhaHashGerada,
                PlanoId = null
            };

            try
            {
                _usuarioService.Inserir(novoUsuario);

                var token = _tokenService.GerarToken(novoUsuario);

                return Created(string.Empty, new
                {
                    token = token,
                    usuario = new { novoUsuario.Id, novoUsuario.Login, novoUsuario.Tipo }
                });
            }
            catch (System.Exception e)
            {
                return BadRequest(new { message = "Erro ao cadastrar usuário.", error = e.Message });
            }
        }

        [HttpGet]
        public IActionResult Listar()
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
                return StatusCode(403, new { message = "Você não tem permissão para acessar a lista de usuários." });
            }

            var listaUsuarios = _usuarioService.Listar();

            // Limpeza de segurança
            foreach (var usuario in listaUsuarios)
            {
                usuario.SenhaHash = null;
            }

            return Ok(listaUsuarios);
        }

        [HttpGet("id")]
        public IActionResult BuscaId([FromQuery(Name = "id")] int id)
        {
            var currentUserId = GetCurrentUserId();
            var currentUserType = GetCurrentUserType();

            if (currentUserId == null)
            {
                return Unauthorized(new { message = "Você precisa estar logado..." });
            }

            bool isAdmin = currentUserType == "Admin";
            bool isSelf = currentUserId.Value == id;

            if (!isAdmin && !isSelf)
            {
                return StatusCode(403, new { message = "Você não tem permissão para visualizar este perfil." });
            }

            var usuario = _usuarioService.BuscaId(id);

            if (usuario == null)
            {
                return NotFound(new { message = "Usuário não encontrado." });
            }

            usuario.SenhaHash = null;
            return Ok(usuario);
        }

        [HttpPut("{id}")]
        public IActionResult Editar(int id, [FromBody] Usuario usuarioEditado)
        {

            var currentUserId = GetCurrentUserId();
            var currentUserType = GetCurrentUserType();

            if (currentUserId == null)
            {
                return Unauthorized(new { message = "Você precisa estar logado..." });
            }

            bool isAdmin = currentUserType == "Admin";
            bool isSelf = currentUserId.Value == id;

            if (!isAdmin && !isSelf)
            {
                return StatusCode(403, new { message = "Você não tem permissão para editar este perfil." });
            }

            _usuarioService.Editar(id, usuarioEditado);
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
            bool isSelf = currentUserId.Value == id;

            if (!isAdmin && !isSelf)
            {
                return StatusCode(403, new { message = "Você não tem permissão para excluir este perfil." });
            }

            _usuarioService.Excluir(id);
            return NoContent();
        }
    }
}