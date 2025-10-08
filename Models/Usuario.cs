using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LocOn.Models
{
    public class Usuario
    {
        [Key()]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(150)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O login do usuário é obrigatório.")]
        [StringLength(100)]
        public string Login { get; set; }

        [Required(ErrorMessage = "O hash da senha é obrigatório.")]
        [StringLength(255)]
        public string SenhaHash { get; set; }

        [Required(ErrorMessage = "O tipo de usuário é obrigatório.")]
        [StringLength(50)]
        public string Tipo { get; set; } = "Padrao";
        public int? PlanoId { get; set; }

        [ForeignKey("PlanoId")]
        public Plano Plano { get; set; }
    }
}