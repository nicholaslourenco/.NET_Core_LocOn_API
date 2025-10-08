using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LocOn.Models;
using LocOn.Context;
using LocOn.DTOs;
using BCrypt.Net;

namespace LocOn.Services
{
    public class UsuarioService
    {

        private readonly BdContext _context;

        public UsuarioService(BdContext context)
        {
            _context = context;
        }

        // READ
        public List<Usuario> Listar()
        {
            return _context.Usuarios.ToList();
        }

        // POST
        public void Inserir(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
        }

        // GET ID
        public Usuario BuscaId(int id)
        {
            return _context.Usuarios.Find(id);
        }

        // LOGIN
        public Usuario Login(string login, string senha)
        {
            Usuario usuario = _context.Usuarios.FirstOrDefault(u => u.Login == login);

            if (usuario == null)
            {
                return null;
            }

            bool senhaCorreta = BCrypt.Net.BCrypt.Verify(senha, usuario.SenhaHash);

            return senhaCorreta ? usuario : null;
        }

        // UPDATE
        public void Editar(int id, Usuario usuarioEditado)
        {
            Usuario usuarioAntigo = _context.Usuarios.Find(id);

            if (usuarioAntigo == null) return; // Tratamento de erro

            usuarioAntigo.Nome = usuarioEditado.Nome;
            usuarioAntigo.Login = usuarioEditado.Login;

            _context.SaveChanges();
        }

        // DELETE
        public void Excluir(int id)
        {
            var usuarioParaRemover = _context.Usuarios.Find(id);
            if(usuarioParaRemover != null)
            {
                _context.Usuarios.Remove(usuarioParaRemover);
                _context.SaveChanges();
            }
        }
    }
}