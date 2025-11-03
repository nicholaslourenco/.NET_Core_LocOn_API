using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LocOn.DTOs;
using LocOn.Models;
using LocOn.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocOn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FilmeController : BaseApiController
    {
        private readonly FilmeService _filmeService;

        public FilmeController(UsuarioService usuarioService, FilmeService filmeService) : base(usuarioService)
        {
            _filmeService = filmeService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Registrar([FromBody] FilmeRegistroDto filmeDto)
        {
            try
            {
                var filme = new Filme
                {
                    Nome = filmeDto.Nome,
                    Genero = filmeDto.Genero,
                    Classificacao = filmeDto.Classificacao,
                    UrlCartaz = filmeDto.UrlCartaz
                };

                Filme filmeInserido = _filmeService.Inserir(filme);

                return Created(string.Empty, filmeInserido);
            }
            catch (System.Exception e)
            {
                return StatusCode(500, new { message = "Erro ao cadastrar filme. Detalhe: " + e.Message });
            }
        }

        [HttpGet]
        public IActionResult Listar()
        {
            return Ok(_filmeService.Listar());
        }

        [HttpGet("id")]
        public IActionResult BuscaId([FromQuery(Name = "id")] int id)
        {
            return Ok(_filmeService.BuscaId(id));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Editar(int id, [FromBody] FilmeRegistroDto filmeDtoEditado)
        {
            try
            {
                var filmeEditado = new Filme
                {
                    Nome = filmeDtoEditado.Nome,
                    Genero = filmeDtoEditado.Genero,
                    Classificacao = filmeDtoEditado.Classificacao,
                    UrlCartaz = filmeDtoEditado.UrlCartaz
                };
                
                Filme filmeAtualizado = _filmeService.Editar(id, filmeEditado);

                if (filmeAtualizado == null)
                {
                    return NotFound(new { message = "Filme não encontrado." });
                }

                return NoContent();
            }
            catch (System.Exception e)
            {
                return StatusCode(500, new { message = "Erro ao editar filme. Detalhe: " + e.Message }); 
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Excluir(int id)
        {
            try
            {
                _filmeService.Excluir(id);

                return NoContent();
            }
            catch (System.Exception e)
            {
                return StatusCode(500, new { message = "Erro ao excluir filme. Detalhe: " + e.Message });
            }
        }
    }
}