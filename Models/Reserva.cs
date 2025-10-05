using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LocOn.Models
{
    public class Reserva
    {
        [Key()]
        public int Id { get; set; }
        public DateTime DataLocacao { get; set; }
        public int FilmeId { get; set; }
        public virtual Filme? Filme { get; set; }
        public int UsuarioId { get; set; }
        
        [ForeignKey("UsuarioId")]
        public virtual Usuario? Usuario { get; set; }
    }
}