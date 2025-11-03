using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LocOn.Models;
using LocOn.Context;

namespace LocOn.Services
{
    public class FilmeService
    {

        private readonly BdContext _context;

        public FilmeService(BdContext context)
        {
            _context = context;
        }

        // GET
        public List<Filme> Listar()
        {
            IQueryable<Filme> query = _context.Filmes;
            return query.OrderBy(f => f.Nome).ToList();
        }

        // POST - Lembrar do cadastro de imagens
        public Filme Inserir(Filme filme)
        {
            _context.Filmes.Add(filme);
            _context.SaveChanges();
            return filme;
        }

        // GET ID
        public Filme BuscaId(int id)
        {
            return _context.Filmes.Find(id);
        }

        // UPDATE - Não deixar usuário selecionar nova imagem toda vez que for cadastrar
        public Filme Editar(int id, Filme filmeEditado)
        {
            Filme filmeAntigo = _context.Filmes.Find(id);

            if (filmeAntigo == null) return null;

            filmeAntigo.Nome = filmeEditado.Nome;
            filmeAntigo.Genero = filmeEditado.Genero;
            filmeAntigo.Classificacao = filmeEditado.Classificacao;
            filmeAntigo.UrlCartaz = filmeEditado.UrlCartaz;

            _context.SaveChanges();
            return filmeAntigo;
        }

        // DELETE
        public void Excluir(int id)
        {
            Filme filme = _context.Filmes.Find(id);

            if (filme == null)
            {
                return;
            }

            _context.Filmes.Remove(filme);
            _context.SaveChanges();
        }
    }
}