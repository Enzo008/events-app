using Microsoft.Data.SqlClient;
using System.Data;
using events_app.Server.Models;
using events_app.Server.Modules;
using System.Security.Claims;

namespace events_app.Modulos
{
     public class ProveedorDAO
    {
        private ConexionDAO cn = new ConexionDAO();


        public IEnumerable<Proveedor> Buscar(ClaimsIdentity? identity, string? proAno = null, string? proCod = null, string? proNom = null, string? proApe = null, string? proTel = null, string? proPre = null)
        {
            var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

            List<Proveedor> temporal = new List<Proveedor>();
            try
            {
                cn.getcn.Open();

                SqlCommand cmd = new SqlCommand("SP_BUSCAR_PROVEEDOR", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@P_PROANO", string.IsNullOrEmpty(proAno) ? (object)DBNull.Value : proAno);
                cmd.Parameters.AddWithValue("@P_PROCOD", string.IsNullOrEmpty(proCod) ? (object)DBNull.Value : proCod);
                cmd.Parameters.AddWithValue("@P_PRONOM", string.IsNullOrEmpty(proNom) ? (object)DBNull.Value : proNom);
                cmd.Parameters.AddWithValue("@P_PROAPE", string.IsNullOrEmpty(proApe) ? (object)DBNull.Value : proApe);
                cmd.Parameters.AddWithValue("@P_PROTEL", string.IsNullOrEmpty(proTel) ? (object)DBNull.Value : proTel);
                cmd.Parameters.AddWithValue("@P_PROPRE", string.IsNullOrEmpty(proPre) ? (object)DBNull.Value : proPre);
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
                    Proveedor obj = new Proveedor
                    {
                        ProAno = rd["PROANO"].ToString(),
                        ProCod = rd["PROCOD"].ToString(),
                        ProNom = rd["PRONOM"].ToString(),
                        ProApe = rd["PROAPE"].ToString(),
                        ProTel = rd["PROTEL"].ToString(),
                        ProPre = rd["PROPRE"].ToString(),
                        SerCan = rd["SERCAN"].ToString(),
                    };
                    temporal.Add(obj);
                }
            }
            catch (SqlException ex)
            {
                temporal = new List<Proveedor>();
                Console.WriteLine(ex.Message);
            }
            finally
            {
                cn.getcn.Close();
            }
            return temporal;
        }

        public IEnumerable<EventoProveedor> BuscarEventoProveedor(ClaimsIdentity? identity, string? eveAno = null, string? eveCod = null, string? proAno = null, string? proCod = null)
        {
            var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

            List<EventoProveedor> temporal = new List<EventoProveedor>();
            try
            {
                cn.getcn.Open();

                SqlCommand cmd = new SqlCommand("SP_BUSCAR_EVENTO_PROVEEDOR", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@P_EVEANO", string.IsNullOrEmpty(eveAno) ? (object)DBNull.Value : eveAno);
                cmd.Parameters.AddWithValue("@P_EVECOD", string.IsNullOrEmpty(eveCod) ? (object)DBNull.Value : eveCod);
                cmd.Parameters.AddWithValue("@P_PROANO", string.IsNullOrEmpty(proAno) ? (object)DBNull.Value : proAno);
                cmd.Parameters.AddWithValue("@P_PROCOD", string.IsNullOrEmpty(proCod) ? (object)DBNull.Value : proCod);
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
                    EventoProveedor obj = new EventoProveedor
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
                        ProAno = rd["PROANO"].ToString(),
                        ProCod = rd["PROCOD"].ToString(),
                        ProNom = rd["PRONOM"].ToString(),
                        ProApe = rd["PROAPE"].ToString(),
                        ProPre = rd["PROPRE"].ToString(),
                        ProTel = rd["PROTEL"].ToString(),
                        SerCan = rd["SERCAN"].ToString(),
                    };
                    temporal.Add(obj);
                }
            }
            catch (SqlException ex)
            {
                temporal = new List<EventoProveedor>();
                Console.WriteLine(ex.Message);
            }
            finally
            {
                cn.getcn.Close();
            }
            return temporal;
        }

        public (string? message, string? messageType) InsertarEventoProveedor(ClaimsIdentity? identity, EventoProveedor eventoProveedor)
        {
            var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

            string? mensaje = "";
            string? tipoMensaje = "";
            try
            {
                cn.getcn.Open();

                SqlCommand cmd = new SqlCommand("SP_INSERTAR_EVENTO_PROVEEDOR", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@P_EVEANO", eventoProveedor.EveAno);
                cmd.Parameters.AddWithValue("@P_EVECOD", eventoProveedor.EveCod);
                cmd.Parameters.AddWithValue("@P_PROANO", eventoProveedor.ProAno);
                cmd.Parameters.AddWithValue("@P_PROCOD", eventoProveedor.ProCod);
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

        // public (string? message, string? messageType) Modificar(ClaimsIdentity? identity, Evento evento)
        // {
        //     var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

        //     string? mensaje = "";
        //     string? tipoMensaje = "";
        //     try
        //     {
        //         cn.getcn.Open();

        //         SqlCommand cmd = new SqlCommand("SP_MODIFICAR_EVENTO", cn.getcn);
        //         cmd.CommandType = CommandType.StoredProcedure;

        //         cmd.Parameters.AddWithValue("@P_EVEANO", evento.EveAno);
        //         cmd.Parameters.AddWithValue("@P_EVECOD", evento.EveCod);
        //         cmd.Parameters.AddWithValue("@P_EVENOM", evento.EveNom);
        //         cmd.Parameters.AddWithValue("@P_EVEDES", evento.EveDes);
        //         cmd.Parameters.AddWithValue("@P_EVEFEC", evento.EveFec);
        //         cmd.Parameters.AddWithValue("@P_EVEHOR", evento.EveHor);
        //         cmd.Parameters.AddWithValue("@P_EVEPREPLA", evento.EvePrePla);
        //         cmd.Parameters.AddWithValue("@P_EVEPREEJE", evento.EvePreEje);
        //         cmd.Parameters.AddWithValue("@P_UBICOD", evento.UbiCod);
        //         cmd.Parameters.AddWithValue("@P_UBICOD", evento.UbiCod);
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

        //         cmd.ExecuteNonQuery();

        //         mensaje = pDescripcionMensaje.Value.ToString();
        //         tipoMensaje = pTipoMensaje.Value.ToString();
        //     }
        //     catch (SqlException ex)
        //     {
        //         mensaje = ex.Message;
        //         tipoMensaje = "1";
        //     }
        //     finally
        //     {
        //         cn.getcn.Close();
        //     }
        //     return (mensaje, tipoMensaje);
        // }

        public (string? message, string? messageType) EliminarEventoProveedor(ClaimsIdentity? identity, EventoProveedor eventoProveedor)
        {
            var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

            string? mensaje = "";
            string? tipoMensaje = "";
            try
            {
                cn.getcn.Open();

                SqlCommand cmd = new SqlCommand("SP_ELIMINAR_EVENTO_PROVEEDOR", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@P_EVEANO", eventoProveedor.EveAno);
                cmd.Parameters.AddWithValue("@P_EVECOD", eventoProveedor.EveCod);
                cmd.Parameters.AddWithValue("@P_PROANO", eventoProveedor.ProAno);
                cmd.Parameters.AddWithValue("@P_PROCOD", eventoProveedor.ProCod);
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