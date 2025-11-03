using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LocOn.Models;
using LocOn.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocOn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlanoController : BaseApiController
    {
        private readonly PlanoService _planoService;

        public PlanoController(UsuarioService usuarioService, PlanoService planoService) : base(usuarioService)
        {
            _planoService = planoService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Registrar(Plano plano)
        {
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
            return Ok(_planoService.Listar());
        }

        [HttpGet("id")]
        public IActionResult BuscaId([FromQuery(Name = "id")] int id)
        {
            return Ok(_planoService.BuscaId(id));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Editar(int id, [FromBody] Plano planoEditado)
        {
            _planoService.Editar(id, planoEditado);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Excluir(int id)
        {
            _planoService.Excluir(id);
            return NoContent();
        }
    }
}