using Microsoft.Data.SqlClient;
using System.Data;
using events_app.Server.Models;
using events_app.Server.Modules;
using System.Security.Claims;

namespace events_app.Modulos
{
     public class MaterialDAO
    {
        private ConexionDAO cn = new ConexionDAO();

        public IEnumerable<Material> Buscar(ClaimsIdentity? identity, string? matCod = null, string? matNom = null, string? matDes = null, string? matPre = null)
        {
            var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

            List<Material> temporal = new List<Material>();
            try
            {
                cn.getcn.Open();

                SqlCommand cmd = new SqlCommand("SP_BUSCAR_MATERIAL", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@P_MATCOD", string.IsNullOrEmpty(matCod) ? (object)DBNull.Value : matCod);
                cmd.Parameters.AddWithValue("@P_MATNOM", string.IsNullOrEmpty(matNom) ? (object)DBNull.Value : matNom);
                cmd.Parameters.AddWithValue("@P_MATDES", string.IsNullOrEmpty(matDes) ? (object)DBNull.Value : matDes);
                cmd.Parameters.AddWithValue("@P_MATPRE", string.IsNullOrEmpty(matPre) ? (object)DBNull.Value : matPre);
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
                    Material obj = new Material
                    {
                        MatCod = rd["MATCOD"].ToString(),
                        MatNom = rd["MATNOM"].ToString(),
                        MatDes = rd["MATDES"].ToString(),
                        MatPre = rd["MATPRE"].ToString(),
                    };
                    temporal.Add(obj);
                }
            }
            catch (SqlException ex)
            {
                temporal = new List<Material>();
                Console.WriteLine(ex.Message);
            }
            finally
            {
                cn.getcn.Close();
            }
            return temporal;
        }

        public IEnumerable<EventoMaterial> BuscarEventoMaterial(ClaimsIdentity? identity, string? eveAno = null, string? eveCod = null, string? eveMatCan = null, string? matCod = null)
        {
            var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

            List<EventoMaterial> temporal = new List<EventoMaterial>();
            try
            {
                cn.getcn.Open();

                SqlCommand cmd = new SqlCommand("SP_BUSCAR_EVENTO_MATERIAL", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@P_EVEMATCAN", string.IsNullOrEmpty(eveMatCan) ? (object)DBNull.Value : eveMatCan);
                cmd.Parameters.AddWithValue("@P_EVEANO", string.IsNullOrEmpty(eveAno) ? (object)DBNull.Value : eveAno);
                cmd.Parameters.AddWithValue("@P_EVECOD", string.IsNullOrEmpty(eveCod) ? (object)DBNull.Value : eveCod);
                cmd.Parameters.AddWithValue("@P_MATCOD", string.IsNullOrEmpty(matCod) ? (object)DBNull.Value : matCod);
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
                    EventoMaterial obj = new EventoMaterial
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
                        MatCod = rd["MATCOD"].ToString(),
                        MatNom = rd["MATNOM"].ToString(),
                        MatDes = rd["MATDES"].ToString(),
                        MatPre = rd["MATPRE"].ToString(),
                        EveMatCan = rd["EVEMATCAN"].ToString(),
                    };
                    temporal.Add(obj);
                }
            }
            catch (SqlException ex)
            {
                temporal = new List<EventoMaterial>();
                Console.WriteLine(ex.Message);
            }
            finally
            {
                cn.getcn.Close();
            }
            return temporal;
        }

        public (string? message, string? messageType) InsertarEventoMaterial(ClaimsIdentity? identity, EventoMaterial eventoMaterial)
        {
            var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

            string? mensaje = "";
            string? tipoMensaje = "";
            try
            {
                cn.getcn.Open();

                SqlCommand cmd = new SqlCommand("SP_INSERTAR_EVENTO_MATERIAL", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@P_EVEMATCAN", eventoMaterial.EveMatCan);
                cmd.Parameters.AddWithValue("@P_EVEANO", eventoMaterial.EveAno);
                cmd.Parameters.AddWithValue("@P_EVECOD", eventoMaterial.EveCod);
                cmd.Parameters.AddWithValue("@P_MATCOD", eventoMaterial.MatCod);
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

        public (string? message, string? messageType) ModificarEventoMaterial(ClaimsIdentity? identity, EventoMaterial eventoMaterial)
        {
            var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

            string? mensaje = "";
            string? tipoMensaje = "";
            try
            {
                cn.getcn.Open();

                SqlCommand cmd = new SqlCommand("SP_MODIFICAR_EVENTO_MATERIAL", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@P_EVEMATCAN", eventoMaterial.EveMatCan);
                cmd.Parameters.AddWithValue("@P_EVEANO", eventoMaterial.EveAno);
                cmd.Parameters.AddWithValue("@P_EVECOD", eventoMaterial.EveCod);
                cmd.Parameters.AddWithValue("@P_MATCOD", eventoMaterial.MatCod);
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

        public (string? message, string? messageType) EliminarEventoMaterial(ClaimsIdentity? identity, EventoMaterial eventoMaterial)
        {
            var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

            string? mensaje = "";
            string? tipoMensaje = "";
            try
            {
                cn.getcn.Open();

                SqlCommand cmd = new SqlCommand("SP_ELIMINAR_EVENTO_MATERIAL", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@P_EVEANO", eventoMaterial.EveAno);
                cmd.Parameters.AddWithValue("@P_EVECOD", eventoMaterial.EveCod);
                cmd.Parameters.AddWithValue("@P_MATCOD", eventoMaterial.MatCod);
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

        public (string? message, string? messageType) Insertar(ClaimsIdentity? identity, Material material)
        {
            var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

            string? mensaje = "";
            string? tipoMensaje = "";
            try
            {
                cn.getcn.Open();

                SqlCommand cmd = new SqlCommand("SP_INSERTAR_MATERIAL", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@P_MATNOM", material.MatNom);
                cmd.Parameters.AddWithValue("@P_MATDES", material.MatDes);
                cmd.Parameters.AddWithValue("@P_MATPRE", material.MatPre);
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

        public (string? message, string? messageType) Modificar(ClaimsIdentity? identity, Material material)
        {
            var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

            string? mensaje = "";
            string? tipoMensaje = "";
            try
            {
                cn.getcn.Open();

                SqlCommand cmd = new SqlCommand("SP_MODIFICAR_MATERIAL", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@P_MATCOD", material.MatCod);
                cmd.Parameters.AddWithValue("@P_MATNOM", material.MatNom);
                cmd.Parameters.AddWithValue("@P_MATDES", material.MatDes);
                cmd.Parameters.AddWithValue("@P_MATPRE", material.MatPre);
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

        public (string? message, string? messageType) Eliminar(ClaimsIdentity? identity, Material material)
        {
            var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

            string? mensaje = "";
            string? tipoMensaje = "";
            try
            {
                cn.getcn.Open();

                SqlCommand cmd = new SqlCommand("SP_ELIMINAR_MATERIAL", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@P_MATCOD", material.MatCod);
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