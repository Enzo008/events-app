using Microsoft.Data.SqlClient;
using System.Data;
using events_app.Server.Models;
using events_app.Server.Modules;
using System.Security.Claims;

namespace events_app.Modulos
{
     public class UbicacionDAO
    {
        private ConexionDAO cn = new ConexionDAO();

        public IEnumerable<Ubicacion> Buscar(ClaimsIdentity? identity, string? ubiCod = null, string? ubiNom = null, string? ubiDes = null, string? ubiCapPer = null)
        {
            var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

            List<Ubicacion> temporal = new List<Ubicacion>();
            try
            {
                cn.getcn.Open();

                SqlCommand cmd = new SqlCommand("SP_BUSCAR_UBICACION", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@P_UBICOD", string.IsNullOrEmpty(ubiCod) ? (object)DBNull.Value : ubiCod);
                cmd.Parameters.AddWithValue("@P_UBINOM", string.IsNullOrEmpty(ubiNom) ? (object)DBNull.Value : ubiNom);
                cmd.Parameters.AddWithValue("@P_UBIDIR", string.IsNullOrEmpty(ubiDes) ? (object)DBNull.Value : ubiDes);
                cmd.Parameters.AddWithValue("@P_UBICAPPER", string.IsNullOrEmpty(ubiCapPer) ? (object)DBNull.Value : ubiCapPer);
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
                    Ubicacion obj = new Ubicacion
                    {
                        UbiCod = rd["UBICOD"].ToString(),
                        UbiNom = rd["UBINOM"].ToString(),
                        UbiDir = rd["UBIDIR"].ToString(),
                        UbiCapPer = rd["UBICAPPER"].ToString(),
                    };
                    temporal.Add(obj);
                }
            }
            catch (SqlException ex)
            {
                temporal = new List<Ubicacion>();
                Console.WriteLine(ex.Message);
            }
            finally
            {
                cn.getcn.Close();
            }
            return temporal;
        }
    }
}