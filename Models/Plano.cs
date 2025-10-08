using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LocOn.Models
{
    public class Plano
    {
        [Key()]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PrecoMensal { get; set; }

        [Required]
        [StringLength(500)]
        public string Descricao { get; set; }

        [Required]
        public int TelasSimultaneas { get; set; }
        public bool Ativo { get; set; } = true;
    }
}