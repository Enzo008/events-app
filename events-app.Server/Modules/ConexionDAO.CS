using Microsoft.Data.SqlClient;

namespace events_app.Server.Modules
{
    public class ConexionDAO
    {
        //atributo de conexion de alcance local
        private SqlConnection cn = new SqlConnection(@"server=powermas.com.pe; Database=tentuap1_EVENT_GLOW; User=tentuap1_EVENT_GLOW; Password=g634dKg^1; MultipleActiveResultSets=true; TrustServerCertificate=true; Encrypt=false");
        //propiedad donde retorna la conexion
        public SqlConnection getcn { get { return cn; } }

    }
}
