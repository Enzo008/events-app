using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using events_app.Modulos;
using events_app.Server.Models;
using events_app.Server.Modules;

namespace events_app.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UbicacionController : ControllerBase
    {
        private readonly UbicacionDAO _ubicaciones;
         private readonly UsuarioDAO _usuarios;

        public UbicacionController(UbicacionDAO ubicaciones, UsuarioDAO usuarios)
        {
            _ubicaciones = ubicaciones;
            _usuarios = usuarios;
        }


        [HttpGet]
        public dynamic Buscar()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validarToken(identity, _usuarios);

            if (!rToken.success) return Unauthorized(rToken);

            var result = _ubicaciones.Buscar(identity);
            return Ok(result);
        }

    }
}