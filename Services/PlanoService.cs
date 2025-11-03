using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LocOn.Models;
using LocOn.Context;
using Microsoft.AspNetCore.Http.HttpResults;
using Stripe;

namespace LocOn.Services
{
    public class PlanoService
    {
        private readonly BdContext _context;

        public PlanoService(BdContext context)
        {
            _context = context;
        }

        // READ
        public List<Plano> Listar()
        {
            return _context.Planos.ToList();
        }

        // POST
        public void Inserir(Plano plano)
        {
            _context.Planos.Add(plano);
            _context.SaveChanges();
        }

        // UPDATE
        public Plano Editar(int id, Plano planoEditado)
        {
            Plano planoAntigo = _context.Planos.Find(id);

            if (planoAntigo == null) return null;

            planoAntigo.Nome = planoEditado.Nome;
            planoAntigo.PrecoMensal = planoEditado.PrecoMensal;
            planoAntigo.Descricao = planoEditado.Descricao;
            planoAntigo.TelasSimultaneas = planoEditado.TelasSimultaneas;
            planoAntigo.Ativo = planoEditado.Ativo;

            _context.SaveChanges();
            return planoAntigo;
        }

        // BUSCA ID
        public Plano BuscaId(int id)
        {
            return _context.Planos.Find(id);
        }

        // DELETE
        public void Excluir(int id)
        {
            var planoRemover = _context.Planos.Find(id);
            if (planoRemover != null)
            {
                _context.Planos.Remove(planoRemover);
                _context.SaveChanges();
            }
        }
    }
}