using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LocOn.DTOs;
using LocOn.Models;
using LocOn.Services;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;

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

        // Endpoint para Login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO dadosLogin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Usuario usuario = _usuarioService.Login(dadosLogin.Login, dadosLogin.Senha);

            if (usuario == null)
            {
                return Unauthorized(new { message = "Login ou senha inválidos." });
            }

            var token = _tokenService.GerarToken(usuario);

            return Ok(new
            {
                token = token,
                usuario = new { usuario.Id, usuario.Login, usuario.Tipo, usuario.PlanoId }
            });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Listar()
        {
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
        public IActionResult Editar(int id, [FromBody] CadastroDTO usuarioEditado)
        {

            var currentUserId = GetCurrentUserId();
            var currentUserType = GetCurrentUserType();

            bool isAdmin = currentUserType == "Admin";
            bool isSelf = currentUserId.Value == id;

            if (!isAdmin && !isSelf)
            {
                return StatusCode(403, new { message = "Você não tem permissão para editar este perfil." });
            }

            string senhaHashGerada = BCrypt.Net.BCrypt.HashPassword(usuarioEditado.Senha);

            var novoUsuario = new Usuario
            {
                Login = usuarioEditado.Login,
                Nome = usuarioEditado.Nome,
                SenhaHash = senhaHashGerada,
            };

            _usuarioService.Editar(id, novoUsuario);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Excluir(int id)
        {
            var currentUserId = GetCurrentUserId();
            var currentUserType = GetCurrentUserType();

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