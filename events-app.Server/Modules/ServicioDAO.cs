using Microsoft.Data.SqlClient;
using System.Data;
using events_app.Server.Models;
using events_app.Server.Modules;
using System.Security.Claims;

namespace events_app.Modulos
{
     public class ServicioDAO
    {
        private ConexionDAO cn = new ConexionDAO();

        public IEnumerable<Servicio> Buscar(ClaimsIdentity? identity, string? serCod = null, string? serNom = null, string? serDes = null)
        {
            var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

            List<Servicio> temporal = new List<Servicio>();
            try
            {
                cn.getcn.Open();

                SqlCommand cmd = new SqlCommand("SP_BUSCAR_SERVICIO", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@P_SERCOD", string.IsNullOrEmpty(serCod) ? (object)DBNull.Value : serCod);
                cmd.Parameters.AddWithValue("@P_SERNOM", string.IsNullOrEmpty(serNom) ? (object)DBNull.Value : serNom);
                cmd.Parameters.AddWithValue("@P_SERDES", string.IsNullOrEmpty(serDes) ? (object)DBNull.Value : serDes);
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
                    Servicio obj = new Servicio
                    {
                        SerCod = rd["SERCOD"].ToString(),
                        SerNom = rd["SERNOM"].ToString(),
                        SerDes = rd["SERDES"].ToString(),
                    };
                    temporal.Add(obj);
                }
            }
            catch (SqlException ex)
            {
                temporal = new List<Servicio>();
                Console.WriteLine(ex.Message);
            }
            finally
            {
                cn.getcn.Close();
            }
            return temporal;
        }

        public IEnumerable<ProveedorServicio> BuscarServicioProveedor(ClaimsIdentity? identity, string? proAno = null, string? proCod = null, string? serCod = null, string? proSerCos = null)
        {
            var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

            List<ProveedorServicio> temporal = new List<ProveedorServicio>();
            try
            {
                cn.getcn.Open();

                SqlCommand cmd = new SqlCommand("SP_BUSCAR_PROVEEDOR_SERVICIO", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@P_PROANO", string.IsNullOrEmpty(proAno) ? (object)DBNull.Value : proAno);
                cmd.Parameters.AddWithValue("@P_PROCOD", string.IsNullOrEmpty(proCod) ? (object)DBNull.Value : proCod);
                cmd.Parameters.AddWithValue("@P_SERCOD", string.IsNullOrEmpty(serCod) ? (object)DBNull.Value : serCod);
                cmd.Parameters.AddWithValue("@P_PROSERCOS", string.IsNullOrEmpty(proSerCos) ? (object)DBNull.Value : proSerCos);
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
                    ProveedorServicio obj = new ProveedorServicio
                    {
                        ProAno = rd["PROANO"].ToString(),
                        ProCod = rd["PROCOD"].ToString(),
                        SerCod = rd["SERCOD"].ToString(),
                        SerNom = rd["SERNOM"].ToString(),
                        SerDes = rd["SERDES"].ToString(),
                        ProSerCos = rd["PROSERCOS"].ToString(),
                    };
                    temporal.Add(obj);
                }
            }
            catch (SqlException ex)
            {
                temporal = new List<ProveedorServicio>();
                Console.WriteLine(ex.Message);
            }
            finally
            {
                cn.getcn.Close();
            }
            return temporal;
        }

        public (string? message, string? messageType) Insertar(ClaimsIdentity? identity, Servicio servicio)
        {
            var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

            string? mensaje = "";
            string? tipoMensaje = "";
            try
            {
                cn.getcn.Open();

                SqlCommand cmd = new SqlCommand("SP_INSERTAR_SERVICIO", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@P_SERNOM", servicio.SerNom);
                cmd.Parameters.AddWithValue("@P_SERDES", servicio.SerDes);
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

        public (string? message, string? messageType) Modificar(ClaimsIdentity? identity, Servicio servicio)
        {
            var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

            string? mensaje = "";
            string? tipoMensaje = "";
            try
            {
                cn.getcn.Open();

                SqlCommand cmd = new SqlCommand("SP_MODIFICAR_SERVICIO", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@P_SERCOD", servicio.SerCod);
                cmd.Parameters.AddWithValue("@P_SERNOM", servicio.SerNom);
                cmd.Parameters.AddWithValue("@P_SERDES", servicio.SerDes);
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

        public (string? message, string? messageType) Eliminar(ClaimsIdentity? identity, Servicio servicio)
        {
            var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

            string? mensaje = "";
            string? tipoMensaje = "";
            try
            {
                cn.getcn.Open();

                SqlCommand cmd = new SqlCommand("SP_ELIMINAR_SERVICIO", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@P_SERCOD", servicio.SerCod);
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