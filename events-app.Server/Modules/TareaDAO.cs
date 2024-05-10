using Microsoft.Data.SqlClient;
using System.Data;
using events_app.Server.Models;
using events_app.Server.Modules;
using System.Security.Claims;

namespace events_app.Modulos
{
     public class TareaDAO
    {
        private ConexionDAO cn = new ConexionDAO();

        public IEnumerable<Tarea> Buscar(ClaimsIdentity? identity, string? tarAno = null, string? tarCod = null, string? tarNom = null, string? tarDes = null, string? tarFecIniPla = null, string? tarFecFinPla = null, string? tarFecIniEje = null, string? tarFecFinEje = null, string? tarEst = null, string? eveAno = null, string? eveCod = null)
        {
            var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

            List<Tarea> temporal = new List<Tarea>();
            try
            {
                cn.getcn.Open();

                SqlCommand cmd = new SqlCommand("SP_BUSCAR_TAREA", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@P_TARANO", string.IsNullOrEmpty(tarAno) ? (object)DBNull.Value : tarAno);
                cmd.Parameters.AddWithValue("@P_TARCOD", string.IsNullOrEmpty(tarCod) ? (object)DBNull.Value : tarCod);
                cmd.Parameters.AddWithValue("@P_TARNOM", string.IsNullOrEmpty(tarNom) ? (object)DBNull.Value : tarNom);
                cmd.Parameters.AddWithValue("@P_TARDES", string.IsNullOrEmpty(tarDes) ? (object)DBNull.Value : tarDes);
                cmd.Parameters.AddWithValue("@P_TARFECINIPLA", string.IsNullOrEmpty(tarFecIniPla) ? (object)DBNull.Value : tarFecIniPla);
                cmd.Parameters.AddWithValue("@P_TARFECFINPLA", string.IsNullOrEmpty(tarFecFinPla) ? (object)DBNull.Value : tarFecFinPla);
                cmd.Parameters.AddWithValue("@P_TARFECINIEJE", string.IsNullOrEmpty(tarFecIniEje) ? (object)DBNull.Value : tarFecIniEje);
                cmd.Parameters.AddWithValue("@P_TARFECFINEJE", string.IsNullOrEmpty(tarFecFinEje) ? (object)DBNull.Value : tarFecFinEje);
                cmd.Parameters.AddWithValue("@P_TAREST", string.IsNullOrEmpty(tarEst) ? (object)DBNull.Value : tarEst);
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
                    Tarea obj = new Tarea
                    {
                        TarAno = rd["TARANO"].ToString(),
                        TarCod = rd["TARCOD"].ToString(),
                        TarNom = rd["TARNOM"].ToString(),
                        TarDes = rd["TARDES"].ToString(),
                        TarFecIniPla = rd["TARFECINIPLA"].ToString(),
                        TarFecFinPla = rd["TARFECFINPLA"].ToString(),
                        TarFecIniEje = rd["TARFECINIEJE"].ToString(),
                        TarFecFinEje = rd["TARFECFINEJE"].ToString(),
                        TarEst = rd["TAREST"].ToString(),
                        EveAno = rd["EVEANO"].ToString(),
                        EveCod = rd["EVECOD"].ToString(),
                    };
                    temporal.Add(obj);
                }
            }
            catch (SqlException ex)
            {
                temporal = new List<Tarea>();
                Console.WriteLine(ex.Message);
            }
            finally
            {
                cn.getcn.Close();
            }
            return temporal;
        }

        public (string? anoOut,string? codOut,string? mensaje, string? tipoMensaje) Insertar(ClaimsIdentity? identity, Tarea tarea)
        {
            var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

            string? mensaje = "";
            string? tipoMensaje = "";
            string? anoOut = "";
            string? codOut = "";
            try
            {
                cn.getcn.Open();

                SqlCommand cmd = new SqlCommand("SP_INSERTAR_TAREA", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@P_TARNOM", tarea.TarNom);
                cmd.Parameters.AddWithValue("@P_TARDES", tarea.TarDes);
                cmd.Parameters.AddWithValue("@P_TARFECINIPLA", tarea.TarFecIniPla);
                cmd.Parameters.AddWithValue("@P_TARFECFINPLA", tarea.TarFecFinPla);
                cmd.Parameters.AddWithValue("@P_TARFECINIEJE", "");
                cmd.Parameters.AddWithValue("@P_TARFECFINEJE", "");
                cmd.Parameters.AddWithValue("@P_TAREST", "I");
                cmd.Parameters.AddWithValue("@P_EVEANO", tarea.EveAno);
                cmd.Parameters.AddWithValue("@P_EVECOD", tarea.EveCod);
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

                SqlParameter pBenAno = new SqlParameter("@P_TARANO_OUT", SqlDbType.NVarChar, 4);
                pBenAno.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pBenAno);

                SqlParameter pBenCod = new SqlParameter("@P_TARCOD_OUT", SqlDbType.Char, 6);
                pBenCod.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pBenCod);

                cmd.ExecuteNonQuery();

                anoOut = pBenAno.Value.ToString();
                codOut = pBenCod.Value.ToString();
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
            return (anoOut, codOut, mensaje, tipoMensaje);
        }

        public (string? mensaje, string? tipoMensaje) Modificar(ClaimsIdentity? identity, Tarea tarea)
        {
            var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

            string? mensaje = "";
            string? tipoMensaje = "";
            try
            {
                cn.getcn.Open();

                SqlCommand cmd = new SqlCommand("SP_MODIFICAR_TAREA", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@P_TARANO", tarea.TarAno);
                cmd.Parameters.AddWithValue("@P_TARCOD", tarea.TarCod);
                cmd.Parameters.AddWithValue("@P_TARNOM", tarea.TarNom);
                cmd.Parameters.AddWithValue("@P_TARDES", tarea.TarDes);
                cmd.Parameters.AddWithValue("@P_TARFECINIPLA", tarea.TarFecIniPla);
                cmd.Parameters.AddWithValue("@P_TARFECFINPLA", tarea.TarFecFinPla);
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

        public (string? message, string? messageType) Eliminar(ClaimsIdentity? identity, Tarea tarea)
        {
            var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

            string? mensaje = "";
            string? tipoMensaje = "";
            try
            {
                cn.getcn.Open();

                SqlCommand cmd = new SqlCommand("SP_ELIMINAR_TAREA", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@P_TARANO", tarea.TarAno);
                cmd.Parameters.AddWithValue("@P_TARCOD", tarea.TarCod);
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