using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using LocOn.Services;
using LocOn.Models;

namespace LocOn.Controllers
{
    public class BaseApiController : ControllerBase
    {
        protected readonly UsuarioService _usuarioService;

        public BaseApiController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        protected int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("id");

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }

            return null;
        }

        protected string GetCurrentUserType()
        {
            var currentUserId = GetCurrentUserId();

            if (!currentUserId.HasValue)
            {
                return null;
            }

            Usuario usuarioLogado = _usuarioService.BuscaId(currentUserId.Value);

            return usuarioLogado?.Tipo;
        }
    }
}