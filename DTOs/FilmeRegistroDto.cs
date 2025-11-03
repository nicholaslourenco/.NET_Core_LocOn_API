using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace LocOn.DTOs
{
    public class FilmeRegistroDto
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O gênero é obrigatório.")]
        public string Genero { get; set; }

        [Required(ErrorMessage = "A classificação é obrigatória.")]
        public string Classificacao { get; set; }

        [Required(ErrorMessage = "A URL do Cartaz é obrigatória.")]
        public string UrlCartaz { get; set; }
    }
}