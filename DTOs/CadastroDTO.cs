using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LocOn.DTOs
{
    public class CadastroDTO
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        [MinLength(6)]
        public string Senha { get; set; }
    }
}