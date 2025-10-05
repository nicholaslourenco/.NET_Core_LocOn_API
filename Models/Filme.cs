using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocOn.Models
{
    public class Filme
    {
        [Key()]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Genero { get; set; }
        public string Classificacao { get; set; }
        public string? CaminhoImagem { get; set; }
        public byte[]? Imagem { get; set; }
    }
}