using Microsoft.Data.SqlClient;
using events_app.Server.Models;
using System.Data;
using System.Text;
using System.Security.Claims;
using System.Transactions;

namespace events_app.Server.Modules
{
    public class UsuarioDAO
    {
        private ConexionDAO cn = new ConexionDAO();

        public IEnumerable<Usuario> Buscar(ClaimsIdentity? identity, string? usuAno = null, string? usuCod = null, string? usuNumDoc = null, string? usuNom = null, string? usuApe = null, string? usuSex = null, string? usuCorEle = null, string? usuTel = null, string? usuNomUsu = null, string? usuPas = null, string? usuEst = null, string? rolCod = null)
        {
            var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

            List<Usuario>? temporal = new List<Usuario>();
            try
            {
                cn.getcn.Open();

                SqlCommand cmd = new SqlCommand("SP_BUSCAR_USUARIO", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;
                // Aquí puedes agregar los parámetros necesarios para tu procedimiento almacenado
                cmd.Parameters.AddWithValue("@P_USUANO", string.IsNullOrEmpty(usuAno) ? (object)DBNull.Value : usuAno);
                cmd.Parameters.AddWithValue("@P_USUCOD", string.IsNullOrEmpty(usuCod) ? (object)DBNull.Value : usuCod);
                cmd.Parameters.AddWithValue("@P_USUNUMDOC", string.IsNullOrEmpty(usuNumDoc) ? (object)DBNull.Value : usuNumDoc);
                cmd.Parameters.AddWithValue("@P_USUNOM", string.IsNullOrEmpty(usuNom) ? (object)DBNull.Value : usuNom);
                cmd.Parameters.AddWithValue("@P_USUAPE", string.IsNullOrEmpty(usuApe) ? (object)DBNull.Value : usuApe);
                cmd.Parameters.AddWithValue("@P_USUSEX", string.IsNullOrEmpty(usuSex) ? (object)DBNull.Value : usuSex);
                cmd.Parameters.AddWithValue("@P_USUCORELE", string.IsNullOrEmpty(usuCorEle) ? (object)DBNull.Value : usuCorEle);
                cmd.Parameters.AddWithValue("@P_USUNOMUSU", string.IsNullOrEmpty(usuNomUsu) ? (object)DBNull.Value : usuNomUsu);
                cmd.Parameters.AddWithValue("@P_USUPAS", string.IsNullOrEmpty(usuPas) ? (object)DBNull.Value : usuPas);
                cmd.Parameters.AddWithValue("@P_USUEST", string.IsNullOrEmpty(usuEst) ? (object)DBNull.Value : usuEst);
                cmd.Parameters.AddWithValue("@P_USUTEL", string.IsNullOrEmpty(usuTel) ? (object)DBNull.Value : usuTel);
                cmd.Parameters.AddWithValue("@P_ROLCOD", "03");
                cmd.Parameters.AddWithValue("@P_LOGIPMAQ", userClaims.UsuIp);
                cmd.Parameters.AddWithValue("@P_USUANO_U", userClaims.UsuAno);
                cmd.Parameters.AddWithValue("@P_USUCOD_U", userClaims.UsuCod);
                cmd.Parameters.AddWithValue("@P_USUNOM_U", userClaims.UsuNom);
                cmd.Parameters.AddWithValue("@P_USUAPE_U", userClaims.UsuApe);

                SqlParameter pDescripcionMensaje = new SqlParameter("@P_DESCRIPCION_MENSAJE", SqlDbType.NVarChar, -1);
                pDescripcionMensaje.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pDescripcionMensaje);

                SqlParameter pTipoMensaje = new SqlParameter("@P_TIPO_MENSAJE", SqlDbType.Char, 1);
                pTipoMensaje.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pTipoMensaje);

                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    Usuario item = new Usuario
                    {
                        UsuAno = rd["USUANO"].ToString(),
                        UsuCod = rd["USUCOD"].ToString(),
                        UsuNom = rd["USUNOM"].ToString(),
                        UsuApe = rd["USUAPE"].ToString(),
                        UsuSex = rd["USUSEX"].ToString(),
                        UsuTel = rd["USUTEL"].ToString(),
                        UsuCorEle = rd["USUCORELE"].ToString(),
                        UsuNumDoc = rd["USUNUMDOC"].ToString(),
                        UsuNomUsu = rd["USUNOMUSU"].ToString(),
                        RolCod = rd["ROLCOD"].ToString(),
                        RolNom = rd["ROLNOM"].ToString(),
                    };
                    temporal.Add(item);
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                cn.getcn.Close();
            }
            return temporal;
        }

        public IEnumerable<UsuarioEvento> BuscarUsuarioEvento(ClaimsIdentity? identity, string? usuAno = null, string? usuCod = null, string? eveAno = null, string? eveCod = null)
        {
            var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

            List<UsuarioEvento>? temporal = new List<UsuarioEvento>();
            try
            {
                cn.getcn.Open();

                SqlCommand cmd = new SqlCommand("SP_BUSCAR_USUARIO_EVENTO", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;
                // Aquí puedes agregar los parámetros necesarios para tu procedimiento almacenado
                cmd.Parameters.AddWithValue("@P_USUANO", string.IsNullOrEmpty(usuAno) ? (object)DBNull.Value : usuAno);
                cmd.Parameters.AddWithValue("@P_USUCOD", string.IsNullOrEmpty(usuCod) ? (object)DBNull.Value : usuCod);
                cmd.Parameters.AddWithValue("@P_EVEANO", string.IsNullOrEmpty(eveAno) ? (object)DBNull.Value : eveAno);
                cmd.Parameters.AddWithValue("@P_EVECOD", string.IsNullOrEmpty(eveCod) ? (object)DBNull.Value : eveCod);
                cmd.Parameters.AddWithValue("@P_LOGIPMAQ", userClaims.UsuIp);
                cmd.Parameters.AddWithValue("@P_USUANO_U", userClaims.UsuAno);
                cmd.Parameters.AddWithValue("@P_USUCOD_U", userClaims.UsuCod);
                cmd.Parameters.AddWithValue("@P_USUNOM_U", userClaims.UsuNom);
                cmd.Parameters.AddWithValue("@P_USUAPE_U", userClaims.UsuApe);

                SqlParameter pDescripcionMensaje = new SqlParameter("@P_DESCRIPCION_MENSAJE", SqlDbType.NVarChar, -1);
                pDescripcionMensaje.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pDescripcionMensaje);

                SqlParameter pTipoMensaje = new SqlParameter("@P_TIPO_MENSAJE", SqlDbType.Char, 1);
                pTipoMensaje.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pTipoMensaje);

                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    UsuarioEvento item = new UsuarioEvento
                    {
                        UsuAno = rd["USUANO"].ToString(),
                        UsuCod = rd["USUCOD"].ToString(),
                       EveAno = rd["EVEANO"].ToString(),
                        EveCod = rd["EVECOD"].ToString(),
                        EveNom = rd["EVENOM"].ToString(),
                        EveDes = rd["EVEDES"].ToString(),
                        EveFec = rd["EVEFEC"].ToString(),
                        EveHor = rd["EVEHOR"].ToString(),
                        EvePrePla = rd["EVEPREPLA"].ToString(),
                        EvePreEje = rd["EVEPREEJE"].ToString(),
                        UbiCod = rd["UBICOD"].ToString(),
                        UbiNom = rd["UBINOM"].ToString(),
                    };
                    temporal.Add(item);
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                cn.getcn.Close();
            }
            return temporal;
        }

        public (string? message, string? messageType) Modificar(ClaimsIdentity? identity, Usuario usuario)
        {
            var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

            string? mensaje = "";
            string? tipoMensaje = "";
            try
            {
                cn.getcn.Open();

                // Obtén el primer carácter de usuario.usuNom
                string? primerCaracterUsuNom = usuario.UsuNom?.Substring(0, 1);

                // Obtén la primera palabra de usuario.usuApe
                string? primeraPalabraUsuApe = usuario.UsuApe?.Split(' ')[0];

                // Concatena el primer carácter de usuario.usuNom y la primera palabra de usuario.usuApe
                string usuNomUsu = primerCaracterUsuNom + primeraPalabraUsuApe;

                SqlCommand cmd = new SqlCommand("SP_MODIFICAR_USUARIO", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;
                // Aquí debes agregar todos los parámetros que necesita tu procedimiento almacenado
                cmd.Parameters.AddWithValue("@P_USUANO", usuario.UsuAno);
                cmd.Parameters.AddWithValue("@P_USUCOD", usuario.UsuCod);
                cmd.Parameters.AddWithValue("@P_USUNUMDOC", usuario.UsuNumDoc);
                cmd.Parameters.AddWithValue("@P_USUNOM", usuario.UsuNom);
                cmd.Parameters.AddWithValue("@P_USUAPE", usuario.UsuApe);
                cmd.Parameters.AddWithValue("@P_USUFECNAC", usuario.UsuFecNac);
                cmd.Parameters.AddWithValue("@P_USUSEX", usuario.UsuSex);
                cmd.Parameters.AddWithValue("@P_USUCORELE", usuario.UsuCorEle);
                cmd.Parameters.AddWithValue("@P_USUFECINC", usuario.UsuFecInc);
                cmd.Parameters.AddWithValue("@P_USUTEL", usuario.UsuTel);
                cmd.Parameters.AddWithValue("@P_USUNOMUSU", usuNomUsu);
                cmd.Parameters.AddWithValue("@P_USUPAS", usuario.UsuPas);
                cmd.Parameters.AddWithValue("@P_USUEST", usuario.UsuEst);
                cmd.Parameters.AddWithValue("@P_ROLCOD", usuario.RolCod);
                cmd.Parameters.AddWithValue("@P_USUMOD", userClaims.UsuNomUsu);
                cmd.Parameters.AddWithValue("@P_LOGIPMAQ", userClaims.UsuIp);
                cmd.Parameters.AddWithValue("@P_USUANO_U", userClaims.UsuAno);
                cmd.Parameters.AddWithValue("@P_USUCOD_U", userClaims.UsuCod);
                cmd.Parameters.AddWithValue("@P_USUNOM_U", userClaims.UsuNom);
                cmd.Parameters.AddWithValue("@P_USUAPE_U", userClaims.UsuApe);

                SqlParameter pDescripcionMensaje = new SqlParameter("@P_DESCRIPCION_MENSAJE", SqlDbType.NVarChar, -1);
                pDescripcionMensaje.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pDescripcionMensaje);

                SqlParameter pTipoMensaje = new SqlParameter("@P_TIPO_MENSAJE", SqlDbType.Char, 1);
                pTipoMensaje.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pTipoMensaje);

                cmd.ExecuteNonQuery();

                mensaje = pDescripcionMensaje.Value.ToString();
                tipoMensaje = pTipoMensaje.Value.ToString();
            }
            catch (SqlException ex)
            {
                mensaje = ex.Message;
            }
            finally
            {
                cn.getcn.Close();
            }
            return (mensaje, tipoMensaje);
        }

        // public (string? message, string? messageType, Usuario? user) ModificarPerfilUsuario(ClaimsIdentity? identity, Usuario usuario)
        // {
        //     var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

        //     string? mensaje = "";
        //     string? tipoMensaje = "";
        //     Usuario? user = new Usuario();
        //     try
        //     {
        //         SqlCommand cmd;
        //         SqlParameter pDescripcionMensaje;
        //         SqlParameter pTipoMensaje;

        //         cn.getcn.Open();

        //         // Obtén el primer carácter de usuario.usuNom
        //         string? primerCaracterUsuNom = usuario.UsuNom?.Substring(0, 1);

        //         // Obtén la primera palabra de usuario.usuApe
        //         string? primeraPalabraUsuApe = usuario.UsuApe?.Split(' ')[0];

        //         // Concatena el primer carácter de usuario.usuNom y la primera palabra de usuario.usuApe
        //         string usuNomUsu = primerCaracterUsuNom + primeraPalabraUsuApe;

        //         cmd = new SqlCommand("SP_MODIFICAR_PERFIL_USUARIO", cn.getcn);
        //         cmd.CommandType = CommandType.StoredProcedure;
        //         // Aquí debes agregar todos los parámetros que necesita tu procedimiento almacenado
        //         cmd.Parameters.AddWithValue("@P_USUANO", usuario.UsuAno);
        //         cmd.Parameters.AddWithValue("@P_USUCOD", usuario.UsuCod);
        //         cmd.Parameters.AddWithValue("@P_DOCIDECOD", usuario.DocIdeCod);
        //         cmd.Parameters.AddWithValue("@P_USUNUMDOC", usuario.UsuNumDoc);
        //         cmd.Parameters.AddWithValue("@P_USUNOM", usuario.UsuNom);
        //         cmd.Parameters.AddWithValue("@P_USUAPE", usuario.UsuApe);
        //         cmd.Parameters.AddWithValue("@P_USUFECNAC", usuario.UsuFecNac);
        //         cmd.Parameters.AddWithValue("@P_USUTEL", usuario.UsuTel);
        //         cmd.Parameters.AddWithValue("@P_USUNOMUSU", usuNomUsu);
        //         cmd.Parameters.AddWithValue("@P_CARCOD", usuario.CarCod);
        //         cmd.Parameters.AddWithValue("@P_USUMOD", userClaims.UsuNomUsu);
        //         cmd.Parameters.AddWithValue("@P_LOGIPMAQ", userClaims.UsuIp);
        //         cmd.Parameters.AddWithValue("@P_USUANO_U", userClaims.UsuAno);
        //         cmd.Parameters.AddWithValue("@P_USUCOD_U", userClaims.UsuCod);
        //         cmd.Parameters.AddWithValue("@P_USUNOM_U", userClaims.UsuNom);
        //         cmd.Parameters.AddWithValue("@P_USUAPE_U", userClaims.UsuApe);

        //         pDescripcionMensaje = new SqlParameter("@P_DESCRIPCION_MENSAJE", SqlDbType.NVarChar, -1);
        //         pDescripcionMensaje.Direction = ParameterDirection.Output;
        //         cmd.Parameters.Add(pDescripcionMensaje);

        //         pTipoMensaje = new SqlParameter("@P_TIPO_MENSAJE", SqlDbType.Char, 1);
        //         pTipoMensaje.Direction = ParameterDirection.Output;
        //         cmd.Parameters.Add(pTipoMensaje);

        //         cmd.ExecuteNonQuery();

        //         mensaje = pDescripcionMensaje.Value.ToString();
        //         tipoMensaje = pTipoMensaje.Value.ToString();

        //         cmd = new SqlCommand("SP_BUSCAR_USUARIO_AUTH", cn.getcn);
        //         cmd.CommandType = CommandType.StoredProcedure;
        //         cmd.Parameters.AddWithValue("@Ano", usuario.UsuAno);
        //         cmd.Parameters.AddWithValue("@Cod", usuario.UsuCod);

        //         StringBuilder jsonResult = new StringBuilder();
        //         SqlDataReader reader = cmd.ExecuteReader();
        //         if (!reader.HasRows)
        //         {
        //             jsonResult.Append("[]");
        //         }
        //         else
        //         {
        //             while (reader.Read())
        //             {
        //                 jsonResult.Append(reader.GetValue(0).ToString());
        //             }
        //         }
        //         // Deserializa la cadena JSON en una lista de objetos Usuario
        //         List<Usuario>? usuarios = JsonConvert.DeserializeObject<List<Usuario>>(jsonResult.ToString());
        //         if (usuarios?.Count > 0)
        //         {
        //             user = usuarios[0];
        //         }
        //     }
        //     catch (SqlException ex)
        //     {
        //         tipoMensaje = "1";
        //         mensaje = ex.Message;
        //     }
        //     finally
        //     {
        //         cn.getcn.Close();
        //     }
        //     return (mensaje, tipoMensaje, user);
        // }

        // public (string? message, string? messageType, Usuario? user) ModificarAvatarUsuario(ClaimsIdentity? identity, Usuario usuario)
        // {
        //     var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

        //     string? mensaje = "";
        //     string? tipoMensaje = "";
        //     Usuario? user = new Usuario();
        //     try
        //     {
        //         SqlCommand cmd;
        //         SqlParameter pDescripcionMensaje;
        //         SqlParameter pTipoMensaje;

        //         cn.getcn.Open();

        //         cmd = new SqlCommand("SP_MODIFICAR_AVATAR_USUARIO", cn.getcn);
        //         cmd.CommandType = CommandType.StoredProcedure;
        //         // Aquí debes agregar todos los parámetros que necesita tu procedimiento almacenado
        //         cmd.Parameters.AddWithValue("@P_USUANO", usuario.UsuAno);
        //         cmd.Parameters.AddWithValue("@P_USUCOD", usuario.UsuCod);
        //         cmd.Parameters.AddWithValue("@P_USUAVA", usuario.UsuAva);
        //         cmd.Parameters.AddWithValue("@P_USUMOD", "");
        //         cmd.Parameters.AddWithValue("@P_LOGIPMAQ", "");
        //         cmd.Parameters.AddWithValue("@P_USUANO_U", "");
        //         cmd.Parameters.AddWithValue("@P_USUCOD_U", "");
        //         cmd.Parameters.AddWithValue("@P_USUNOM_U", "");
        //         cmd.Parameters.AddWithValue("@P_USUAPE_U", "");

        //         pDescripcionMensaje = new SqlParameter("@P_DESCRIPCION_MENSAJE", SqlDbType.NVarChar, -1);
        //         pDescripcionMensaje.Direction = ParameterDirection.Output;
        //         cmd.Parameters.Add(pDescripcionMensaje);

        //         pTipoMensaje = new SqlParameter("@P_TIPO_MENSAJE", SqlDbType.Char, 1);
        //         pTipoMensaje.Direction = ParameterDirection.Output;
        //         cmd.Parameters.Add(pTipoMensaje);

        //         cmd.ExecuteNonQuery();

        //         mensaje = pDescripcionMensaje.Value.ToString();
        //         tipoMensaje = pTipoMensaje.Value.ToString();

        //         cmd = new SqlCommand("SP_BUSCAR_USUARIO_AUTH", cn.getcn);
        //         cmd.CommandType = CommandType.StoredProcedure;
        //         cmd.Parameters.AddWithValue("@Ano", usuario.UsuAno);
        //         cmd.Parameters.AddWithValue("@Cod", usuario.UsuCod);

        //         StringBuilder jsonResult = new StringBuilder();
        //         SqlDataReader reader = cmd.ExecuteReader();
        //         if (!reader.HasRows)
        //         {
        //             jsonResult.Append("[]");
        //         }
        //         else
        //         {
        //             while (reader.Read())
        //             {
        //                 jsonResult.Append(reader.GetValue(0).ToString());
        //             }
        //         }
        //         // Deserializa la cadena JSON en una lista de objetos Usuario
        //         List<Usuario>? usuarios = JsonConvert.DeserializeObject<List<Usuario>>(jsonResult.ToString());
        //         if (usuarios?.Count > 0)
        //         {
        //             user = usuarios[0];
        //         }
        //     }
        //     catch (SqlException ex)
        //     {
        //         tipoMensaje = "1";
        //         mensaje = ex.Message;
        //     }
        //     finally
        //     {
        //         cn.getcn.Close();
        //     }
        //     return (mensaje, tipoMensaje, user);
        // }

        public (string? message, string? messageType) InsertarUsuarioEvento(ClaimsIdentity? identity, UsuarioEvento usuarioEvento)
        {
            var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

            string? mensaje = "";
            string? tipoMensaje = "";
            try
            {
                cn.getcn.Open();

                SqlCommand cmd = new SqlCommand("SP_INSERTAR_USUARIO_EVENTO", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@P_USUANO", usuarioEvento.UsuAno);
                cmd.Parameters.AddWithValue("@P_USUCOD", usuarioEvento.UsuCod);
                cmd.Parameters.AddWithValue("@P_EVEANO", usuarioEvento.EveAno);
                cmd.Parameters.AddWithValue("@P_EVECOD", usuarioEvento.EveCod);
                cmd.Parameters.AddWithValue("@P_USUING", userClaims.UsuNomUsu);
                cmd.Parameters.AddWithValue("@P_LOGIPMAQ", userClaims.UsuIp);
                cmd.Parameters.AddWithValue("@P_USUANO_U", userClaims.UsuAno);
                cmd.Parameters.AddWithValue("@P_USUCOD_U", userClaims.UsuCod);
                cmd.Parameters.AddWithValue("@P_USUNOM_U", userClaims.UsuNom);
                cmd.Parameters.AddWithValue("@P_USUAPE_U", userClaims.UsuApe);

                SqlParameter pDescripcionMensaje = new SqlParameter("@P_DESCRIPCION_MENSAJE", SqlDbType.NVarChar, -1);
                pDescripcionMensaje.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pDescripcionMensaje);

                SqlParameter pTipoMensaje = new SqlParameter("@P_TIPO_MENSAJE", SqlDbType.Char, 1);
                pTipoMensaje.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pTipoMensaje);

                cmd.ExecuteNonQuery();

                mensaje = pDescripcionMensaje.Value.ToString();
                tipoMensaje = pTipoMensaje.Value.ToString();
            }
            catch (SqlException ex)
            {
                mensaje = ex.Message;
                tipoMensaje = "1";
            }
            finally
            {
                cn.getcn.Close();
            }
            return (mensaje, tipoMensaje);
        }

        public (string? mensaje, string? tipoMensaje, string? usuAnoOut, string? usuCodOut) Insertar(Usuario usuario)
        {
            string? mensaje = "";
            string? tipoMensaje = "";
            string? usuAnoOut = "";
            string? usuCodOut = "";
            
            try
            {
                cn.getcn.Open();

                // Obtén el primer carácter de usuario.usuNom
                string? primerCaracterUsuNom = usuario.UsuNom?.Substring(0, 1);

                // Obtén la primera palabra de usuario.usuApe
                string? primeraPalabraUsuApe = usuario.UsuApe?.Split(' ')[0];

                // Concatena el primer carácter de usuario.usuNom y la primera palabra de usuario.usuApe
                string usuNomUsu = primerCaracterUsuNom + primeraPalabraUsuApe;

                SqlCommand cmd = new SqlCommand("SP_INSERTAR_USUARIO", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@P_USUNUMDOC", usuario.UsuNumDoc);
                cmd.Parameters.AddWithValue("@P_USUNOM", usuario.UsuNom);
                cmd.Parameters.AddWithValue("@P_USUAPE", usuario.UsuApe);
                cmd.Parameters.AddWithValue("@P_USUSEX", usuario.UsuSex);
                cmd.Parameters.AddWithValue("@P_USUCORELE", usuario.UsuCorEle);
                cmd.Parameters.AddWithValue("@P_USUTEL", usuario.UsuTel);
                cmd.Parameters.AddWithValue("@P_USUNOMUSU", usuNomUsu);
                cmd.Parameters.AddWithValue("@P_USUPAS", usuario.UsuPas);
                cmd.Parameters.AddWithValue("@P_USUEST", "A");
                cmd.Parameters.AddWithValue("@P_ROLCOD", "02");
                cmd.Parameters.AddWithValue("@P_USUING", usuNomUsu);
                cmd.Parameters.AddWithValue("@P_LOGIPMAQ", usuario.UsuIp);
                cmd.Parameters.AddWithValue("@P_USUNOM_U", usuario.UsuNom);
                cmd.Parameters.AddWithValue("@P_USUAPE_U", usuario.UsuApe);

                SqlParameter pDescripcionMensaje = new SqlParameter("@P_DESCRIPCION_MENSAJE", SqlDbType.NVarChar, -1);
                pDescripcionMensaje.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pDescripcionMensaje);

                SqlParameter pTipoMensaje = new SqlParameter("@P_TIPO_MENSAJE", SqlDbType.Char, 1);
                pTipoMensaje.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pTipoMensaje);

                SqlParameter pUsuAno = new SqlParameter("@P_USUANO_OUT", SqlDbType.NVarChar, 4);
                pUsuAno.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pUsuAno);

                SqlParameter pUsuCod = new SqlParameter("@P_USUCOD_OUT", SqlDbType.Char, 6);
                pUsuCod.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pUsuCod);

                cmd.ExecuteNonQuery();

                usuAnoOut = pUsuAno.Value.ToString();
                usuCodOut = pUsuCod.Value.ToString();
                mensaje = pDescripcionMensaje.Value.ToString();
                tipoMensaje = pTipoMensaje.Value.ToString();
            }
            catch (SqlException ex)
            {
                mensaje = ex.Message;
            }
            finally
            {
                cn.getcn.Close();
            }
            return (usuAnoOut, usuCodOut, mensaje, tipoMensaje);
        }

        public (string? message, string? messageType) RestablecerPassword(ClaimsIdentity? identity, Usuario usuario)
        {
            var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

            string? mensaje = "";
            string? tipoMensaje = "";
            try
            {
                cn.getcn.Open();

                SqlCommand cmd = new SqlCommand("SP_RESTABLECER_PASSWORD", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@P_USUANO", usuario.UsuAno);
                cmd.Parameters.AddWithValue("@P_USUCOD", usuario.UsuCod);
                cmd.Parameters.AddWithValue("@P_USUPAS", usuario.UsuPas);

                cmd.ExecuteNonQuery();

                mensaje = "Se restableció la contraseña";
                tipoMensaje = "3";
            }
            catch (SqlException ex)
            {
                mensaje = ex.Message;
                tipoMensaje = "1";
            }
            finally
            {
                cn.getcn.Close();
            }
            return (mensaje, tipoMensaje);
        }

        public (string? message, string? messageType) RestablecerPasswordOlvidada(Usuario usuario)
        {
            string? mensaje = "";
            string? tipoMensaje = "";
            try
            {
                cn.getcn.Open();

                SqlCommand cmd = new SqlCommand("SP_RESTABLECER_PASSWORD", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@P_USUANO", usuario.UsuAno);
                cmd.Parameters.AddWithValue("@P_USUCOD", usuario.UsuCod);
                cmd.Parameters.AddWithValue("@P_USUPAS", usuario.UsuPas);

                cmd.ExecuteNonQuery();

                mensaje = "Se restableció la contraseña";
                tipoMensaje = "3";
            }
            catch (SqlException ex)
            {
                mensaje = ex.Message;
                tipoMensaje = "1";
            }
            finally
            {
                cn.getcn.Close();
            }
            return (mensaje, tipoMensaje);
        }

        // public IEnumerable<Usuario> BuscarUsuarioPorCorreo(string usuCorEle)
        // {
        //     List<Usuario>? temporal = new List<Usuario>();
        //     try
        //     {
        //         cn.getcn.Open();

        //         SqlCommand cmd = new SqlCommand("SP_VALIDAR_EMAIL", cn.getcn);
        //         cmd.CommandType = CommandType.StoredProcedure;
        //         cmd.Parameters.AddWithValue("@P_USUCORELE", usuCorEle);

        //         StringBuilder jsonResult = new StringBuilder();
        //         SqlDataReader reader = cmd.ExecuteReader();
        //         if (!reader.HasRows)
        //         {
        //             jsonResult.Append("[]");
        //         }
        //         else
        //         {
        //             while (reader.Read())
        //             {
        //                 jsonResult.Append(reader.GetValue(0).ToString());
        //             }
        //         }
        //         // Deserializa la cadena JSON en una lista de objetos Usuario
        //         temporal = JsonConvert.DeserializeObject<List<Usuario>>(jsonResult.ToString());
        //     }
        //     catch (SqlException ex)
        //     {
        //         Console.WriteLine(ex.Message);
        //     }
        //     finally
        //     {
        //         cn.getcn.Close();
        //     }
        //     return temporal?? new List<Usuario>();
        // }

        // public IEnumerable<Usuario> Buscar(ClaimsIdentity? identity, string? usuAno = null, string? usuCod = null, string? docIdeCod = null, string? usuNumDoc = null, string? usuNom = null, string? usuApe = null, string? usuFecNac = null, string? usuSex = null, string? usuCorEle = null, string? usuCarCod = null, string? usuFecInc = null, string? usuTel = null, string? usuNomUsu = null, string? usuPas = null, string? usuEst = null, string? rolCod = null, string? ubiAno = null, string? ubiCod = null)
        // {
        //     var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

        //     List<Usuario>? temporal = new List<Usuario>();
        //     try
        //     {
        //         cn.getcn.Open();

        //         SqlCommand cmd = new SqlCommand("SP_BUSCAR_USUARIO", cn.getcn);
        //         cmd.CommandType = CommandType.StoredProcedure;
        //         // Aquí puedes agregar los parámetros necesarios para tu procedimiento almacenado
        //         cmd.Parameters.AddWithValue("@P_USUANO", string.IsNullOrEmpty(usuAno) ? (object)DBNull.Value : usuAno);
        //         cmd.Parameters.AddWithValue("@P_USUCOD", string.IsNullOrEmpty(usuCod) ? (object)DBNull.Value : usuCod);
        //         cmd.Parameters.AddWithValue("@P_DOCIDECOD", string.IsNullOrEmpty(docIdeCod) ? (object)DBNull.Value : docIdeCod);
        //         cmd.Parameters.AddWithValue("@P_USUNUMDOC", string.IsNullOrEmpty(usuNumDoc) ? (object)DBNull.Value : usuNumDoc);
        //         cmd.Parameters.AddWithValue("@P_USUNOM", string.IsNullOrEmpty(usuNom) ? (object)DBNull.Value : usuNom);
        //         cmd.Parameters.AddWithValue("@P_USUAPE", string.IsNullOrEmpty(usuApe) ? (object)DBNull.Value : usuApe);
        //         cmd.Parameters.AddWithValue("@P_USUFECNAC", string.IsNullOrEmpty(usuFecNac) ? (object)DBNull.Value : usuFecNac);
        //         cmd.Parameters.AddWithValue("@P_USUSEX", string.IsNullOrEmpty(usuSex) ? (object)DBNull.Value : usuSex);
        //         cmd.Parameters.AddWithValue("@P_USUCORELE", string.IsNullOrEmpty(usuCorEle) ? (object)DBNull.Value : usuCorEle);
        //         cmd.Parameters.AddWithValue("@P_CARCOD", string.IsNullOrEmpty(usuCarCod) ? (object)DBNull.Value : usuCarCod);
        //         cmd.Parameters.AddWithValue("@P_USUFECINC", string.IsNullOrEmpty(usuFecInc) ? (object)DBNull.Value : usuFecInc);
        //         cmd.Parameters.AddWithValue("@P_USUTEL", string.IsNullOrEmpty(usuTel) ? (object)DBNull.Value : usuTel);
        //         cmd.Parameters.AddWithValue("@P_USUNOMUSU", string.IsNullOrEmpty(usuNomUsu) ? (object)DBNull.Value : usuNomUsu);
        //         cmd.Parameters.AddWithValue("@P_USUPAS", string.IsNullOrEmpty(usuPas) ? (object)DBNull.Value : usuPas);
        //         cmd.Parameters.AddWithValue("@P_USUEST", string.IsNullOrEmpty(usuEst) ? (object)DBNull.Value : usuEst);
        //         cmd.Parameters.AddWithValue("@P_ROLCOD", string.IsNullOrEmpty(rolCod) ? (object)DBNull.Value : rolCod);
        //         cmd.Parameters.AddWithValue("@P_UBIANO", string.IsNullOrEmpty(ubiAno) ? (object)DBNull.Value : ubiAno);
        //         cmd.Parameters.AddWithValue("@P_UBICOD", string.IsNullOrEmpty(ubiCod) ? (object)DBNull.Value : ubiCod);
        //         cmd.Parameters.AddWithValue("@P_LOGIPMAQ", userClaims.UsuIp);
        //         cmd.Parameters.AddWithValue("@P_USUANO_U", userClaims.UsuAno);
        //         cmd.Parameters.AddWithValue("@P_USUCOD_U", userClaims.UsuCod);
        //         cmd.Parameters.AddWithValue("@P_USUNOM_U", userClaims.UsuNom);
        //         cmd.Parameters.AddWithValue("@P_USUAPE_U", userClaims.UsuApe);

        //         SqlParameter pDescripcionMensaje = new SqlParameter("@P_DESCRIPCION_MENSAJE", SqlDbType.NVarChar, -1);
        //         pDescripcionMensaje.Direction = ParameterDirection.Output;
        //         cmd.Parameters.Add(pDescripcionMensaje);

        //         SqlParameter pTipoMensaje = new SqlParameter("@P_TIPO_MENSAJE", SqlDbType.Char, 1);
        //         pTipoMensaje.Direction = ParameterDirection.Output;
        //         cmd.Parameters.Add(pTipoMensaje);

        //         StringBuilder jsonResult = new StringBuilder();
        //         SqlDataReader reader = cmd.ExecuteReader();
        //         if (!reader.HasRows)
        //         {
        //             jsonResult.Append("[]");
        //         }
        //         else
        //         {
        //             while (reader.Read())
        //             {
        //                 jsonResult.Append(reader.GetValue(0).ToString());
        //             }
        //         }
        //         // Deserializa la cadena JSON en una lista de objetos Usuario
        //         temporal = JsonConvert.DeserializeObject<List<Usuario>>(jsonResult.ToString());
        //     }
        //     catch (SqlException ex)
        //     {
        //         Console.WriteLine(ex.Message);
        //     }
        //     finally
        //     {
        //         cn.getcn.Close();
        //     }
        //     return temporal?? new List<Usuario>();
        // }

        // public bool TienePermiso(string usuAno, string usuCod, string perNom)
        // {
        //     bool tienePermiso = false;

        //     try
        //     {
        //         cn.getcn.Open();

        //         SqlCommand cmd = new SqlCommand("SP_VERIFICAR_PERMISO", cn.getcn);
        //         cmd.CommandType = CommandType.StoredProcedure;
        //         cmd.Parameters.AddWithValue("@UsuAno", usuAno);
        //         cmd.Parameters.AddWithValue("@UsuCod", usuCod);
        //         cmd.Parameters.AddWithValue("@PerNom", perNom);

        //         SqlDataReader rd = cmd.ExecuteReader();
        //         if (rd.Read())
        //         {
        //             tienePermiso = rd.GetInt32(0) > 0;
        //         }
        //         rd.Close();
        //     }
        //     catch (SqlException ex)
        //     {
        //         Console.WriteLine(ex.Message);
        //     }
        //     finally
        //     {
        //         cn.getcn.Close();
        //     }

        //     return tienePermiso;
        // }

        public IEnumerable<Usuario>  BuscarUsuario(string? ano, string? cod)
        {
            List<Usuario> temporal = new List<Usuario>();

            try
            {
                cn.getcn.Open();

                SqlCommand cmd = new SqlCommand("SP_BUSCAR_USUARIO_AUTH", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_USUANO", ano);
                cmd.Parameters.AddWithValue("@P_USUCOD", cod);

                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    Usuario item = new Usuario
                    {
                        UsuAno = rd["USUANO"].ToString(),
                        UsuCod = rd["USUCOD"].ToString(),
                        UsuNom = rd["USUNOM"].ToString(),
                        UsuApe = rd["USUAPE"].ToString(),
                        UsuNomUsu = rd["USUNOMUSU"].ToString(),
                        UsuSex = rd["USUSEX"].ToString(),
                        UsuAva = rd.IsDBNull(rd.GetOrdinal("USUAVA")) ? null : rd.GetSqlBinary(rd.GetOrdinal("USUAVA")).Value,
                        RolCod = rd["ROLCOD"].ToString(),
                        RolNom = rd["ROLNOM"].ToString(),
                    };
                    temporal.Add(item);
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                cn.getcn.Close();
            }

            return temporal;
        }


        // public Usuario BuscarUsuarioLog(string ano, string cod)
        // {
        //     List<Usuario>? usuarios = null;
        //     Usuario? usuario = null;

        //     try
        //     {
        //         SqlCommand cmd = new SqlCommand("SP_BUSCAR_USUARIO_AUTH", cn.getcn);
        //         cmd.CommandType = CommandType.StoredProcedure;
        //         cmd.Parameters.AddWithValue("@Ano", ano);
        //         cmd.Parameters.AddWithValue("@Cod", cod);

        //         StringBuilder jsonResult = new StringBuilder();
        //         SqlDataReader reader = cmd.ExecuteReader();
        //         if (!reader.HasRows)
        //         {
        //             jsonResult.Append("[]");
        //         }
        //         else
        //         {
        //             while (reader.Read())
        //             {
        //                 jsonResult.Append(reader.GetValue(0).ToString());
        //             }
        //         }
        //         // Deserializa la cadena JSON en una lista de objetos Usuario
        //         usuarios = JsonConvert.DeserializeObject<List<Usuario>>(jsonResult.ToString());
        //         if (usuarios.Count > 0)
        //         {
        //             usuario = usuarios[0];
        //         }
        //     }
        //     catch (SqlException ex)
        //     {
        //         Console.WriteLine(ex.Message);
        //     }
        //     finally
        //     {
        //     }

        //     return usuario?? new Usuario();
        // }

        public IEnumerable<Usuario> ValidarUsuario(string email, string password)
        {
            List<Usuario> temporal = new List<Usuario>();

            try
            {
                cn.getcn.Open();

                SqlCommand cmd = new SqlCommand("SP_VALIDAR_USUARIO", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_USUCORELE", email);
                cmd.Parameters.AddWithValue("@P_USUPAS", password);

                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    Usuario item = new Usuario
                    {
                        UsuAno = rd["USUANO"].ToString(),
                        UsuCod = rd["USUCOD"].ToString(),
                        UsuNom = rd["USUNOM"].ToString(),
                        UsuApe = rd["USUAPE"].ToString(),
                        UsuNomUsu = rd["USUNOMUSU"].ToString(),
                        UsuSex = rd["USUSEX"].ToString(),
                        UsuAva = rd.IsDBNull(rd.GetOrdinal("USUAVA")) ? null : rd.GetSqlBinary(rd.GetOrdinal("USUAVA")).Value,
                        RolCod = rd["ROLCOD"].ToString(),
                        RolNom = rd["ROLNOM"].ToString(),
                    };
                    temporal.Add(item);
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                cn.getcn.Close();
            }

            return temporal;
        }

        public (string? message, string? messageType) Eliminar(ClaimsIdentity? identity, string usuAno, string usuCod)
        {
            var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

            string? mensaje = "";
            string? tipoMensaje = "";
            try
            {
                cn.getcn.Open();

                SqlCommand cmd = new SqlCommand("SP_ELIMINAR_USUARIO", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@P_USUANO", usuAno);
                cmd.Parameters.AddWithValue("@P_USUCOD", usuCod);
                cmd.Parameters.AddWithValue("@P_USUMOD", userClaims.UsuNomUsu);
                cmd.Parameters.AddWithValue("@P_LOGIPMAQ", userClaims.UsuIp);
                cmd.Parameters.AddWithValue("@P_USUANO_U", userClaims.UsuAno);
                cmd.Parameters.AddWithValue("@P_USUCOD_U", userClaims.UsuCod);
                cmd.Parameters.AddWithValue("@P_USUNOM_U", userClaims.UsuNom);
                cmd.Parameters.AddWithValue("@P_USUAPE_U", userClaims.UsuApe);

                SqlParameter pDescripcionMensaje = new SqlParameter("@P_DESCRIPCION_MENSAJE", SqlDbType.NVarChar, -1);
                pDescripcionMensaje.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pDescripcionMensaje);

                SqlParameter pTipoMensaje = new SqlParameter("@P_TIPO_MENSAJE", SqlDbType.Char, 1);
                pTipoMensaje.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pTipoMensaje);

                cmd.ExecuteNonQuery();

                mensaje = pDescripcionMensaje.Value.ToString();
                tipoMensaje = pTipoMensaje.Value.ToString();
            }
            catch (SqlException ex)
            {
                mensaje = ex.Message;
            }
            finally
            {
                cn.getcn.Close();
            }
            return (mensaje, tipoMensaje);
        }

    }
}
