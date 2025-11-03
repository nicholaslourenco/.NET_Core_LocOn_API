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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            string senhaAdminHash = "$2b$10$4hSe9mGQEjgdJxkTez2gbu7eRJ0ghBT0lSZ55pWjxyxSKg5/7/YNS";

            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 1,
                    Login = "Admin",
                    Nome = "Administrador Master Blaster",
                    Tipo = "Admin",
                    SenhaHash = senhaAdminHash,
                    PlanoId = null
                }
            );
        }
    }
}