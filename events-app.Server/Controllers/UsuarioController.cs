﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using events_app.Server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using events_app.Server.Modules;

namespace events_app.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioDAO _usuarios;
        public IConfiguration _configuration { get; set; }

        public UsuarioController(IConfiguration configuration, UsuarioDAO usuarios) { 
            _configuration = configuration;
            _usuarios = usuarios;
        }

        [HttpGet]
        public dynamic Buscar()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validarToken(identity, _usuarios);

            if (!rToken.success) return Unauthorized(rToken);
           
            var usuarios = _usuarios.Buscar(identity);
            return Ok(usuarios);
        }

        [HttpGet]
        [Route("evento/{usuAno}/{usuCod}")]
        public dynamic BuscarUsuarioEvento(string usuAno, string usuCod)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validarToken(identity, _usuarios);

            if (!rToken.success) return Unauthorized(rToken);
           
            var usuarios = _usuarios.BuscarUsuarioEvento(identity,usuAno:usuAno,usuCod:usuCod);
            return Ok(usuarios);
        }

        [HttpPost]
        [Route("evento")]
        public dynamic InsertarUsuarioEvento(UsuarioEvento usuarioEvento)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validarToken(identity, _usuarios);

            if (!rToken.success) return rToken;

            var (message, messageType) = _usuarios.InsertarUsuarioEvento(identity, usuarioEvento);
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

        [HttpPost]
        public dynamic Insertar(Usuario usuario)
        {
            string? passwordHash = usuario.UsuPas;

            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(usuario.UsuPas));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                usuario.UsuPas = builder.ToString();
            }

            Console.WriteLine(passwordHash);

            var (usuAno, usuCod, message, messageType) = _usuarios.Insertar(usuario);
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
                usuario.UsuPas = passwordHash;
                var loginResult = IniciarSesion(usuario);
                return loginResult;
            }
        }

        [HttpPut("{usuAno}/{usuCod}")]
        public dynamic Modificar(string usuAno, string usuCod, [FromBody] Usuario usuario)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validarToken(identity, _usuarios);

            if (!rToken.success) return rToken;

            usuario.UsuAno = usuAno;
            usuario.UsuCod = usuCod;
            var (message, messageType) = _usuarios.Modificar(identity, usuario);
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

        // [HttpPut("profile")]
        // public dynamic ModificarPerfilUsuario(Usuario usuario)
        // {
        //     var identity = HttpContext.User.Identity as ClaimsIdentity;
        //     var rToken = Jwt.validarToken(identity, _usuarios);

        //     if (!rToken.success) return rToken;

        //     var (message, messageType, user) = _usuarios.ModificarPerfilUsuario(identity, usuario);
        //     if (messageType == "1") // Error
        //     {
        //         return new BadRequestObjectResult(new { success = false, message });
        //     }
        //     else if (messageType == "2") // Registro ya existe
        //     {
        //         return new ConflictObjectResult(new { success = false, message });
        //     }
        //     else // Registro modificado correctamente
        //     {
        //         return new OkObjectResult(new { success = true, message, user });
        //     }
        // }

        // [HttpPost("update-avatar")]
        // public dynamic ModificarAvatarUsuario(Usuario usuario)
        // {
        //     var identity = HttpContext.User.Identity as ClaimsIdentity;

        //     var (message, messageType, user) = _usuarios.ModificarAvatarUsuario(identity, usuario);
        //     if (messageType == "1") // Error
        //     {
        //         return new BadRequestObjectResult(new { success = false, message });
        //     }
        //     else if (messageType == "2") // Registro ya existe
        //     {
        //         return new ConflictObjectResult(new { success = false, message });
        //     }
        //     else // Registro modificado correctamente
        //     {
        //         return new OkObjectResult(new { success = true, message, user });
        //     }
        // }

        // [HttpPut]
        // [Route("restablecerPassword")]
        // public dynamic RestablecerPassword(Usuario usuario)
        // {
        //     var identity = HttpContext.User.Identity as ClaimsIdentity;
        //     var rToken = Jwt.validarToken(identity, _usuarios);

        //     if (!rToken.success) return rToken;

        //     dynamic data = rToken.result;
        //     Usuario usuarioActual = new Usuario
        //     {
        //         UsuAno = data.UsuAno,
        //         UsuCod = data.UsuCod,
        //         RolCod = data.RolCod
        //     };
        //     using (SHA256 sha256Hash = SHA256.Create())
        //     {
        //         byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(usuario.UsuPas));
        //         StringBuilder builder = new StringBuilder();
        //         for (int i = 0; i < bytes.Length; i++)
        //         {
        //             builder.Append(bytes[i].ToString("x2"));
        //         }
        //         usuario.UsuPas = builder.ToString();
        //     }
        //     var (message, messageType) = _usuarios.RestablecerPassword(identity, usuario);
        //     if (messageType == "1") // Error
        //     {
        //         return new BadRequestObjectResult(new { success = false, message });
        //     }
        //     else if (messageType == "2") // Registro ya existe
        //     {
        //         return new ConflictObjectResult(new { success = false, message });
        //     }
        //     else // Registro modificado correctamente
        //     {
        //         return new OkObjectResult(new {success = true, message });
        //     }
        // }

        // [HttpPut]
        // [Route("forgot-restablecer")]
        // public dynamic RestablecerPasswordOlvidada(Usuario usuario)
        // {
        //     Console.WriteLine(usuario.UsuPas);
        //     using (SHA256 sha256Hash = SHA256.Create())
        //     {
        //         byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(usuario.UsuPas));
        //         StringBuilder builder = new StringBuilder();
        //         for (int i = 0; i < bytes.Length; i++)
        //         {
        //             builder.Append(bytes[i].ToString("x2"));
        //         }
        //         usuario.UsuPas = builder.ToString();
        //     }
        //     Console.WriteLine(usuario.UsuPas);
        //     var (message, messageType) = _usuarios.RestablecerPasswordOlvidada(usuario);
        //     if (messageType == "1") // Error
        //     {
        //         return new BadRequestObjectResult(new { success = false, message });
        //     }
        //     else if (messageType == "2") // Registro ya existe
        //     {
        //         return new ConflictObjectResult(new { success = false, message });
        //     }
        //     else // Registro modificado correctamente
        //     {
        //         return new OkObjectResult(new {success = true, message });
        //     }
        // }


        // [HttpGet]
        // public dynamic Buscar()
        // {
        //     var identity = HttpContext.User.Identity as ClaimsIdentity;
        //     var rToken = Jwt.validarToken(identity, _usuarios);

        //     if (!rToken.success) return Unauthorized(rToken);
           
        //     var usuarios = _usuarios.Buscar(identity);
        //     return Ok(usuarios);
        // }

        // [HttpGet]
        // [Route("tecnico")]
        // public dynamic BuscarUsuariosTecnico()
        // {
        //     var identity = HttpContext.User.Identity as ClaimsIdentity;
        //     var rToken = Jwt.validarToken(identity, _usuarios);

        //     if (!rToken.success) return Unauthorized(rToken);

        //     var usuarios = _usuarios.Listado(identity, rolCod: "04");
        //     return Ok(usuarios);
        // }

        // [HttpGet]
        // [Route("forgot-password/{usuCorEle}")]
        // public dynamic BuscarUsuarioPorCorreo(string usuCorEle)
        // {
        //     var result = _usuarios.BuscarUsuarioPorCorreo(usuCorEle);
        //     return Ok(result);
        // }

        // [HttpGet]
        // [Route("{usuAno}/{usuCod}")]
        // public dynamic ObtenerUsuario(string usuAno, string usuCod)
        // {
        //     var identity = HttpContext.User.Identity as ClaimsIdentity;
        //     var rToken = Jwt.validarToken(identity, _usuarios);

        //     if (!rToken.success) return Unauthorized(rToken);
           
        //     var usuarios = _usuarios.Buscar(identity, usuAno, usuCod);
        //     var usuarioData = usuarios.FirstOrDefault();
        //     return Ok(usuarioData);
        // }


        [HttpPost]
        [Route("login")]
        public dynamic IniciarSesion(Usuario usuario)
        {
            // Haz hash de la contraseña proporcionada
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(usuario.UsuPas));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                usuario.UsuPas = builder.ToString();
            }

            // Recorrer usuarios y validar si hay un usuario con ese email
            var usuarioValidado = _usuarios.ValidarUsuario(usuario.UsuCorEle, usuario.UsuPas).FirstOrDefault();
            if (usuarioValidado == null)
            {
                return new BadRequestObjectResult(new { 
                    success = false, 
                    message = "Credenciales incorrectas o usuario inactivo"
                });
            }

            var jwt = _configuration.GetSection("Jwt").Get<Jwt>();

            var now = DateTime.UtcNow;
            var secondsSinceEpoch = Math.Round((now - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds);


            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, secondsSinceEpoch.ToString(), ClaimValueTypes.Integer64),
                new Claim("USUANO", usuarioValidado.UsuAno),
                new Claim("USUCOD", usuarioValidado.UsuCod),
                new Claim("USUIP", usuario.UsuIp),
                new Claim("USUNOM", usuarioValidado.UsuNom),
                new Claim("USUAPE", usuarioValidado.UsuApe),
                new Claim("USUNOMUSU", usuarioValidado.UsuNomUsu)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                jwt.Issuer,
                jwt.Audience,
                claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: singIn
            );

            return new
            {
                success = true,
                result = new JwtSecurityTokenHandler().WriteToken(token),
                user = usuarioValidado,
                message = "Usuario autenticado correctamente"
            };
        }


        [HttpGet]
        [Route("perfil")]
        public dynamic ValidarUsuario()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            dynamic rToken = Jwt.validarToken(identity, _usuarios);

            if (!rToken.success) // Error
            {
                return new UnauthorizedObjectResult(rToken);
            } 
            else // Registro modificado correctamente
            {
                return new OkObjectResult(rToken);
            }
        }

        // [HttpDelete]
        // [Route("{usuAno}/{usuCod}")]
        // public dynamic Eliminar(string usuAno, string usuCod)
        // {
        //     var identity = HttpContext.User.Identity as ClaimsIdentity;
        //     var rToken = Jwt.validarToken(identity, _usuarios);

        //     if (!rToken.success) return rToken;

        //     dynamic data = rToken.result;
        //     Usuario usuario = new Usuario
        //     {
        //         UsuAno = data.UsuAno,
        //         UsuCod = data.UsuCod,
        //         RolCod = data.RolCod
        //     };
        //     if (!_usuarios.TienePermiso(usuario.UsuAno, usuario.UsuCod, "ELIMINAR ESTADO") && usuario.RolCod != "01")
        //     {
        //         return new
        //         {
        //             success = false,
        //             message = "No tienes permisos para eliminar estados",
        //             result = ""
        //         };
        //     }

        //     var (message, messageType) = _usuarios.Eliminar(identity, usuAno, usuCod);
        //     if (messageType == "1") // Error
        //     {
        //         return new BadRequestObjectResult(new { success = false, message });
        //     }
        //     else if (messageType == "2") // Registro ya existe
        //     {
        //         return new ConflictObjectResult(new { success = false, message });
        //     }
        //     else // Registro modificado correctamente
        //     {
        //         return new OkObjectResult(new {success = true, message });
        //     }
        // }

        // [HttpGet]
        // [Route("access/{usuAno}/{usuCod}")]
        // public dynamic BuscarMetasUsuario(string usuAno, string usuCod)
        // {
        //     var identity = HttpContext.User.Identity as ClaimsIdentity;
        //     var rToken = Jwt.validarToken(identity, _usuarios);

        //     if (!rToken.success) return Unauthorized(rToken);

        //     var result = _usuarios.BuscarUsuarioAcceso(identity, usuAno:usuAno, usuCod:usuCod);
        //     return Ok(result);
        // }

        // [HttpPost]
        // [Route("access")]
        // public dynamic InsertarMetaUsuario(UsuarioAccesoDto usuarioAccesoDto)
        // {
        //     var identity = HttpContext.User.Identity as ClaimsIdentity;
        //     var rToken = Jwt.validarToken(identity, _usuarios);

        //     var (message, messageType) = _usuarios.InsertarUsuarioAcceso(identity, usuarioAccesoDto.AccesosInsertar, usuarioAccesoDto.AccesosEliminar);
        //     if (messageType == "1")
        //     {
        //         return new BadRequestObjectResult(new { success = false, message });
        //     }
        //     else if (messageType == "2")
        //     {
        //         return new ConflictObjectResult(new { success = false, message });
        //     }
        //     else
        //     {
        //         return new OkObjectResult(new { success = true, message });
        //     }
        // }


    }
}
