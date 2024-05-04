using Microsoft.Data.SqlClient;
using System.Data;
using events_app.Server.Models;
using events_app.Server.Modules;
using System.Security.Claims;

namespace events_app.Modulos
{
     public class MenuDAO
    {
        private ConexionDAO cn = new ConexionDAO();

        // public bool InsertarMenuUsuario(string usuAno, string usuCod, string menAno, string menCod)
        // {
        //     try
        //     {
        //         cn.getcn.Open();

        //         SqlCommand cmd = new SqlCommand("SP_INSERTAR_MENU_USUARIO", cn.getcn);
        //         cmd.CommandType = CommandType.StoredProcedure;
        //         cmd.Parameters.AddWithValue("@USUANO", usuAno);
        //         cmd.Parameters.AddWithValue("@USUCOD", usuCod);
        //         cmd.Parameters.AddWithValue("@MENANO", menAno);
        //         cmd.Parameters.AddWithValue("@MENCOD", menCod);
        //         cmd.ExecuteNonQuery();

        //         return true;
        //     }
        //     catch (SqlException ex)
        //     {
        //         Console.WriteLine(ex.Message);
        //         return false;
        //     }
        //     finally
        //     {
        //         cn.getcn.Close();
        //     }
        // }

        // public bool EliminarMenuUsuario(string usuAno, string usuCod, string menAno, string menCod)
        // {
        //     try
        //     {
        //         cn.getcn.Open();

        //         SqlCommand cmd = new SqlCommand("SP_ELIMINAR_MENU_USUARIO", cn.getcn);
        //         cmd.CommandType = CommandType.StoredProcedure;
        //         cmd.Parameters.AddWithValue("@USUANO", usuAno);
        //         cmd.Parameters.AddWithValue("@USUCOD", usuCod);
        //         cmd.Parameters.AddWithValue("@MENANO", menAno);
        //         cmd.Parameters.AddWithValue("@MENCOD", menCod);
        //         cmd.ExecuteNonQuery();

        //         return true;
        //     }
        //     catch (SqlException ex)
        //     {
        //         Console.WriteLine(ex.Message);
        //         return false;
        //     }
        //     finally
        //     {
        //         cn.getcn.Close();
        //     }
        // }
        // public IEnumerable<Menu> ListadoMenuPorUsuario(string usuAno, string usuCod)
        // {
        //     List<Menu> temporal = new List<Menu>();
        //     try
        //     {
        //         cn.getcn.Open();

        //         SqlCommand cmd = new SqlCommand("SP_LISTAR_MENU_POR_USUARIO", cn.getcn);
        //         cmd.CommandType = CommandType.StoredProcedure;
        //         cmd.Parameters.AddWithValue("@UsuAno", usuAno);
        //         cmd.Parameters.AddWithValue("@UsuCod", usuCod);
        //         SqlDataReader rd = cmd.ExecuteReader();
        //         while (rd.Read())
        //         {
        //             temporal.Add(new Menu()
        //             {
        //                 MenAno = rd.GetString(0),
        //                 MenCod = rd.GetString(1),
        //                 MenNom = rd.GetString(2),
        //                 MenRef = rd.GetString(3),
        //                 MenIco = rd.IsDBNull(4) ? null : rd.GetString(4),
        //                 MenOrd = rd.IsDBNull(5) ? null : rd.GetString(5),
        //                 MenAnoPad = rd.IsDBNull(6) ? null : rd.GetString(6),
        //                 MenCodPad = rd.IsDBNull(7) ? null : rd.GetString(7),
        //                 UsuIng = rd.GetString(8),
        //                 FecIng = rd.IsDBNull(9) ? (DateTime?)null : rd.GetDateTime(9),
        //                 UsuMod = rd.GetString(10),
        //                 FecMod = rd.IsDBNull(11) ? (DateTime?)null : rd.GetDateTime(11),
        //                 EstReg = rd.GetString(12)[0]
        //             });
        //         }
        //         rd.Close();
        //     }
        //     catch (SqlException ex)
        //     {
        //         temporal = new List<Menu>();
        //         Console.WriteLine(ex.Message);
        //     }
        //     finally
        //     {
        //         cn.getcn.Close();
        //     }
        //     return temporal;
        // }


        // public IEnumerable<Menu> ListadoMenuPorRol(string rolCod)
        // {
        //     List<Menu> temporal = new List<Menu>();
        //     try
        //     {
        //         cn.getcn.Open();

        //         SqlCommand cmd = new SqlCommand("SP_LISTAR_MENU_POR_ROL", cn.getcn);
        //         cmd.CommandType = CommandType.StoredProcedure;
        //         cmd.Parameters.AddWithValue("@RolCod", rolCod);
        //         SqlDataReader rd = cmd.ExecuteReader();
        //         while (rd.Read())
        //         {
        //             temporal.Add(new Menu()
        //             {
        //                 MenAno = rd.GetString(0),
        //                 MenCod = rd.GetString(1),
        //                 MenNom = rd.GetString(2),
        //                 MenRef = rd.GetString(3),
        //                 MenAnoPad = rd.IsDBNull(4) ? null : rd.GetString(4),
        //                 MenCodPad = rd.IsDBNull(5) ? null : rd.GetString(5),
        //                 UsuIng = rd.GetString(6),
        //                 FecIng = rd.IsDBNull(7) ? (DateTime?)null : rd.GetDateTime(7),
        //                 UsuMod = rd.GetString(8),
        //                 FecMod = rd.IsDBNull(9) ? (DateTime?)null : rd.GetDateTime(9),
        //                 EstReg = rd.GetString(10)[0],
        //                 MenIco = rd.IsDBNull(11) ? null : rd.GetString(11)
        //             });
        //         }
        //         rd.Close();
        //     }
        //     catch (SqlException ex)
        //     {
        //         temporal = new List<Menu>();
        //         Console.WriteLine(ex.Message);
        //     }
        //     finally
        //     {
        //         cn.getcn.Close();
        //     }
        //     return temporal;
        // }

        public IEnumerable<Menu> Buscar(ClaimsIdentity? identity, string? menAno = null, string? menCod = null, string? menNom = null, string? menRef = null, string? menIco = null, string? menOrd = null, string? menAnoPad = null, string? menCodPad = null)
        {
            var userClaims = new UserClaims().GetClaimsFromIdentity(identity);

            List<Menu> temporal = new List<Menu>();
            try
            {
                cn.getcn.Open();

                SqlCommand cmd = new SqlCommand("SP_BUSCAR_MENU", cn.getcn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@P_MENANO", string.IsNullOrEmpty(menAno) ? (object)DBNull.Value : menAno);
                cmd.Parameters.AddWithValue("@P_MENCOD", string.IsNullOrEmpty(menCod) ? (object)DBNull.Value : menCod);
                cmd.Parameters.AddWithValue("@P_MENNOM", string.IsNullOrEmpty(menNom) ? (object)DBNull.Value : menNom);
                cmd.Parameters.AddWithValue("@P_MENREF", string.IsNullOrEmpty(menRef) ? (object)DBNull.Value : menRef);
                cmd.Parameters.AddWithValue("@P_MENICO", string.IsNullOrEmpty(menIco) ? (object)DBNull.Value : menIco);
                cmd.Parameters.AddWithValue("@P_MENORD", string.IsNullOrEmpty(menOrd) ? (object)DBNull.Value : menOrd);
                cmd.Parameters.AddWithValue("@P_MENANOPAD", string.IsNullOrEmpty(menAnoPad) ? (object)DBNull.Value : menAnoPad);
                cmd.Parameters.AddWithValue("@P_MENCODPAD", string.IsNullOrEmpty(menCodPad) ? (object)DBNull.Value : menCodPad);
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
                    Menu menu = new Menu
                    {
                        MenAno = rd["MENANO"].ToString(),
                        MenCod = rd["MENCOD"].ToString(),
                        MenNom = rd["MENNOM"].ToString(),
                        MenRef = rd["MENREF"].ToString(),
                        MenIco = rd["MENICO"].ToString(),
                        MenOrd = rd["MENORD"].ToString(),
                        MenAnoPad = rd["MENANOPAD"] == DBNull.Value ? null : rd["MENANOPAD"].ToString(),
                        MenCodPad = rd["MENCODPAD"] == DBNull.Value ? null : rd["MENCODPAD"].ToString(),
                    };
                    temporal.Add(menu);
                }
            }
            catch (SqlException ex)
            {
                temporal = new List<Menu>();
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