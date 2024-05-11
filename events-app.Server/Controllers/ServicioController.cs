using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using events_app.Modulos;
using events_app.Server.Models;
using events_app.Server.Modules;

namespace events_app.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicioController : ControllerBase
    {
        private readonly ServicioDAO _servicios;
         private readonly UsuarioDAO _usuarios;

        public ServicioController(ServicioDAO servicios, UsuarioDAO usuarios)
        {
            _servicios = servicios;
            _usuarios = usuarios;
        }


        [HttpGet]
        public dynamic Buscar()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validarToken(identity, _usuarios);

            if (!rToken.success) return Unauthorized(rToken);

            var result = _servicios.Buscar(identity);
            return Ok(result);
        }

        [HttpGet]
        [Route("proveedor/{proAno}/{proCod}")]
        public dynamic BuscarTareasEvento(string proAno, string proCod)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validarToken(identity, _usuarios);

            if (!rToken.success) return Unauthorized(rToken);

            var result = _servicios.BuscarServicioProveedor(identity, proAno: proAno, proCod: proCod);
            return Ok(result);
        }

         [HttpPost]
        public dynamic Insertar(Servicio material)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validarToken(identity, _usuarios);

            if (!rToken.success) return rToken;

            var (message, messageType)= _servicios.Insertar(identity, material);
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

        [HttpPut]
        public dynamic Modificar(Servicio servicio)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validarToken(identity, _usuarios);

            if (!rToken.success) return rToken;

            var (message, messageType) = _servicios.Modificar(identity, servicio);
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
        public dynamic Eliminar(Servicio servicio)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validarToken(identity, _usuarios);

            if (!rToken.success) return rToken;

            var (message, messageType) = _servicios.Eliminar(identity, servicio);
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