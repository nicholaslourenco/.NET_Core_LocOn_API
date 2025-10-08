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
            // Removido o 'using (BdContext context = new BdContext())'
            IQueryable<Filme> query = _context.Filmes;
            return query.OrderBy(f => f.Nome).ToList();
        }

        // POST - Lembrar do cadastro de imagens
        public void Inserir(Filme filme)
        {
            _context.Filmes.Add(filme);
            _context.SaveChanges();
        }

        // GET ID
        public Filme BuscaId(int id)
        {
            return _context.Filmes.Find(id);
        }

        // UPDATE - Não deixar usuário selecionar nova imagem toda vez que for cadastrar
        public void Editar(int id, Filme filmeEditado)
        {
            // O uso do Find e SaveChanges continua correto.
            Filme filmeAntigo = _context.Filmes.Find(id);

            // Verificação de segurança:
            if(filmeAntigo == null) return; // Adicione tratamento de erro se o filme não for encontrado

            filmeAntigo.Nome = filmeEditado.Nome;
            filmeAntigo.Genero = filmeEditado.Genero;
            filmeAntigo.Classificacao = filmeEditado.Classificacao;
            
            // Para não sobrescrever a imagem antiga, use:
            if(!string.IsNullOrEmpty(filmeEditado.CaminhoImagem)) 
            {
                filmeAntigo.CaminhoImagem = filmeEditado.CaminhoImagem;
            } 

            _context.SaveChanges();
        }

        // DELETE
        public void Excluir(int id)
        {
            var filmeParaRemover = _context.Filmes.Find(id);
            if(filmeParaRemover != null)
            {
                _context.Filmes.Remove(filmeParaRemover);
                _context.SaveChanges();
            }
        }
    }
}