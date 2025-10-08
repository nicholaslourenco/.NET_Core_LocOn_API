using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LocOn.Models;
using Microsoft.EntityFrameworkCore;

namespace LocOn.Context
{
    public class BdContext : DbContext
    {
        
        public BdContext(DbContextOptions<BdContext> options) : base(options)
        {
        }
        
        public DbSet<Filme> Filmes { get; set; }
        public DbSet<Plano> Planos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}