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
        public static int admin = 0;
        public static int padrao = 1;
        [Key()]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public int Tipo { get; set; }
        public ICollection<Reserva>? Reservas { get; set; }
    }
}