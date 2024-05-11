using Microsoft.Data.SqlClient;
using System.Data;
using events_app.Server.Models;
using events_app.Server.Modules;
using System.Security.Claims;

namespace events_app.Modulos
{
     public class EventoDAO
    {
        private ConexionDAO cn = new ConexionDAO();

        public IEnumerable<Evento> Buscar(ClaimsIdentity? identity, string? eveAno = null, string? eveCod = null, string? eveNom = null, string? eveDes = null, string? eveFec = null, string? eveHor = null, string? evePrePla = null, string? evePreEje = null, string? ubiCod = null, string? eveEst = null)
        {
            var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

            List<Evento> temporal = new List<Evento>();
            try
            {
                cn.getcn.Open();

                SqlCommand cmd = new SqlCommand("SP_BUSCAR_EVENTO", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@P_EVEANO", string.IsNullOrEmpty(eveAno) ? (object)DBNull.Value : eveAno);
                cmd.Parameters.AddWithValue("@P_EVECOD", string.IsNullOrEmpty(eveCod) ? (object)DBNull.Value : eveCod);
                cmd.Parameters.AddWithValue("@P_EVENOM", string.IsNullOrEmpty(eveNom) ? (object)DBNull.Value : eveNom);
                cmd.Parameters.AddWithValue("@P_EVEDES", string.IsNullOrEmpty(eveDes) ? (object)DBNull.Value : eveDes);
                cmd.Parameters.AddWithValue("@P_EVEFEC", string.IsNullOrEmpty(eveFec) ? (object)DBNull.Value : eveFec);
                cmd.Parameters.AddWithValue("@P_EVEHOR", string.IsNullOrEmpty(eveHor) ? (object)DBNull.Value : eveHor);
                cmd.Parameters.AddWithValue("@P_EVEPREPLA", string.IsNullOrEmpty(evePrePla) ? (object)DBNull.Value : evePrePla);
                cmd.Parameters.AddWithValue("@P_EVEPREEJE", string.IsNullOrEmpty(evePreEje) ? (object)DBNull.Value : evePreEje);
                cmd.Parameters.AddWithValue("@P_UBICOD", string.IsNullOrEmpty(ubiCod) ? (object)DBNull.Value : ubiCod);
                cmd.Parameters.AddWithValue("@P_EVEEST", string.IsNullOrEmpty(eveEst) ? (object)DBNull.Value : eveEst);
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
                    Evento obj = new Evento
                    {
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
                    temporal.Add(obj);
                }
            }
            catch (SqlException ex)
            {
                temporal = new List<Evento>();
                Console.WriteLine(ex.Message);
            }
            finally
            {
                cn.getcn.Close();
            }
            return temporal;
        }

        public (string? ano,string? cod,string? message, string? messageType) Insertar(ClaimsIdentity? identity, Evento evento)
        {
            var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

            string? mensaje = "";
            string? tipoMensaje = "";
            string? ano = "";
            string? cod = "";
            SqlCommand cmd;
            SqlParameter pDescripcionMensaje;
            SqlParameter pTipoMensaje;
            try
            {
                cn.getcn.Open();

                cmd = new SqlCommand("SP_INSERTAR_EVENTO", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@P_EVENOM", evento.EveNom);
                cmd.Parameters.AddWithValue("@P_EVEDES", evento.EveDes);
                cmd.Parameters.AddWithValue("@P_EVEFEC", evento.EveFec);
                cmd.Parameters.AddWithValue("@P_EVEHOR", evento.EveHor);
                cmd.Parameters.AddWithValue("@P_EVEPREPLA", evento.EvePrePla);
                cmd.Parameters.AddWithValue("@P_EVEPREEJE", "0");
                cmd.Parameters.AddWithValue("@P_UBICOD", evento.UbiCod);
                cmd.Parameters.AddWithValue("@P_EVEEST", "P");
                cmd.Parameters.AddWithValue("@P_USUING", userClaims.UsuNomUsu);
                cmd.Parameters.AddWithValue("@P_LOGIPMAQ", userClaims.UsuIp);
                cmd.Parameters.AddWithValue("@P_USUANO_U", userClaims.UsuAno);
                cmd.Parameters.AddWithValue("@P_USUCOD_U", userClaims.UsuCod);
                cmd.Parameters.AddWithValue("@P_USUNOM_U", userClaims.UsuNom);
                cmd.Parameters.AddWithValue("@P_USUAPE_U", userClaims.UsuApe);

                pDescripcionMensaje = new SqlParameter("@P_DESCRIPCION_MENSAJE", SqlDbType.NVarChar, -1);
                pDescripcionMensaje.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pDescripcionMensaje);

                pTipoMensaje = new SqlParameter("@P_TIPO_MENSAJE", SqlDbType.Char, 1);
                pTipoMensaje.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pTipoMensaje);

                SqlParameter pBenAno = new SqlParameter("@P_EVEANO_OUT", SqlDbType.NVarChar, 4);
                pBenAno.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pBenAno);

                SqlParameter pBenCod = new SqlParameter("@P_EVECOD_OUT", SqlDbType.Char, 6);
                pBenCod.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pBenCod);

                cmd.ExecuteNonQuery();

                ano = pBenAno.Value.ToString();
                cod = pBenCod.Value.ToString();
                mensaje = pDescripcionMensaje.Value.ToString();
                tipoMensaje = pTipoMensaje.Value.ToString();

                cmd = new SqlCommand("SP_INSERTAR_USUARIO_EVENTO", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@P_USUANO", userClaims.UsuAno);
                cmd.Parameters.AddWithValue("@P_USUCOD", userClaims.UsuCod);
                cmd.Parameters.AddWithValue("@P_EVEANO", ano);
                cmd.Parameters.AddWithValue("@P_EVECOD", cod);
                cmd.Parameters.AddWithValue("@P_USUING", userClaims.UsuNomUsu);
                cmd.Parameters.AddWithValue("@P_LOGIPMAQ", userClaims.UsuIp);
                cmd.Parameters.AddWithValue("@P_USUANO_U", userClaims.UsuAno);
                cmd.Parameters.AddWithValue("@P_USUCOD_U", userClaims.UsuCod);
                cmd.Parameters.AddWithValue("@P_USUNOM_U", userClaims.UsuNom);
                cmd.Parameters.AddWithValue("@P_USUAPE_U", userClaims.UsuApe);

                pDescripcionMensaje = new SqlParameter("@P_DESCRIPCION_MENSAJE", SqlDbType.NVarChar, -1);
                pDescripcionMensaje.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pDescripcionMensaje);

                pTipoMensaje = new SqlParameter("@P_TIPO_MENSAJE", SqlDbType.Char, 1);
                pTipoMensaje.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pTipoMensaje);

                cmd.ExecuteNonQuery();
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
            return (ano, cod, mensaje, tipoMensaje);
        }
        public (string? message, string? messageType) Modificar(ClaimsIdentity? identity, Evento evento)
        {
            var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

            string? mensaje = "";
            string? tipoMensaje = "";
            try
            {
                cn.getcn.Open();

                SqlCommand cmd = new SqlCommand("SP_MODIFICAR_EVENTO", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@P_EVEANO", evento.EveAno);
                cmd.Parameters.AddWithValue("@P_EVECOD", evento.EveCod);
                cmd.Parameters.AddWithValue("@P_EVENOM", evento.EveNom);
                cmd.Parameters.AddWithValue("@P_EVEDES", evento.EveDes);
                cmd.Parameters.AddWithValue("@P_EVEFEC", evento.EveFec);
                cmd.Parameters.AddWithValue("@P_EVEHOR", evento.EveHor);
                cmd.Parameters.AddWithValue("@P_EVEPREPLA", evento.EvePrePla);
                cmd.Parameters.AddWithValue("@P_EVEPREEJE", evento.EvePreEje);
                cmd.Parameters.AddWithValue("@P_UBICOD", evento.UbiCod);
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
                tipoMensaje = "1";
            }
            finally
            {
                cn.getcn.Close();
            }
            return (mensaje, tipoMensaje);
        }

        public (string? message, string? messageType) Eliminar(ClaimsIdentity? identity, Evento evento)
        {
            var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

            string? mensaje = "";
            string? tipoMensaje = "";
            try
            {
                cn.getcn.Open();

                SqlCommand cmd = new SqlCommand("SP_ELIMINAR_EVENTO", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@P_EVEANO", evento.EveAno);
                cmd.Parameters.AddWithValue("@P_EVECOD", evento.EveCod);
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
                tipoMensaje = "1";
            }
            finally
            {
                cn.getcn.Close();
            }
            return (mensaje, tipoMensaje);
        }

    }
}