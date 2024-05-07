using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using events_app.Modulos;
using events_app.Server.Models;
using events_app.Server.Modules;

namespace events_app.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        private readonly EventoDAO _eventos;
         private readonly UsuarioDAO _usuarios;

        public EventoController(EventoDAO eventos, UsuarioDAO usuarios)
        {
            _eventos = eventos;
            _usuarios = usuarios;
        }


        [HttpGet]
        public dynamic Buscar()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validarToken(identity, _usuarios);

            if (!rToken.success) return Unauthorized(rToken);

            var menu = _eventos.Buscar(identity);
            return Ok(menu);
        }

        [HttpPost]
        public dynamic Insertar(Evento evento)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validarToken(identity, _usuarios);

            if (!rToken.success) return rToken;

            var (ano, cod, message, messageType) = _eventos.Insertar(identity, evento);
            if (messageType == "1") // Error
            {
                return new BadRequestObjectResult(new { success = false, message });
            }
            else if (messageType == "2") // Registro ya existe
            {
                return new ConflictObjectResult(new { success = false, message });
            }
            else // Registro modificado correctamente
            {
                return new OkObjectResult(new { ano, cod, success = true, message });
            }
        }

        [HttpPut]
        public dynamic Modificar(Evento evento)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validarToken(identity, _usuarios);

            if (!rToken.success) return rToken;

            var (message, messageType) = _eventos.Modificar(identity, evento);
            if (messageType == "1") // Error
            {
                return new BadRequestObjectResult(new { success = false, message });
            }
            else if (messageType == "2") // Registro ya existe
            {
                return new ConflictObjectResult(new { success = false, message });
            }
            else // Registro modificado correctamente
            {
                return new OkObjectResult(new { success = true, message });
            }
        }

        [HttpDelete]
        public dynamic Eliminar(Evento evento)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validarToken(identity, _usuarios);

            if (!rToken.success) return rToken;

            var (message, messageType) = _eventos.Eliminar(identity, evento);
            if (messageType == "1") // Error
            {
                return new BadRequestObjectResult(new { success = false, message });
            }
            else if (messageType == "2") // Registro ya existe
            {
                return new ConflictObjectResult(new { success = false, message });
            }
            else // Registro modificado correctamente
            {
                return new OkObjectResult(new { success = true, message });
            }
        }

    }
}