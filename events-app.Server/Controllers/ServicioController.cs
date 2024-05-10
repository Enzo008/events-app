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
        [Route("proveedor/{proAno}/{proCod}")]
        public dynamic BuscarTareasEvento(string proAno, string proCod)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validarToken(identity, _usuarios);

            if (!rToken.success) return Unauthorized(rToken);

            var result = _servicios.BuscarServicioProveedor(identity, proAno: proAno, proCod: proCod);
            return Ok(result);
        }
    }
}