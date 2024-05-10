using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace events_app.Server.Models
{
    public class Tarea
    {
        [Key, Column(Order = 0)]
        public String? TarAno { get; set; }

        [Key, Column(Order = 1)]
        public String? TarCod { get; set; }
        public String? TarNom { get; set; }
        public String? TarDes { get; set; }
        public String? TarFecIniPla { get; set; }
        public String? TarFecFinPla { get; set; }
        public String? TarFecIniEje { get; set; }
        public String? TarFecFinEje { get; set; }
        public String? TarEst { get; set; }
        public String? EveAno { get; set; }
        public String? EveCod { get; set; }
        public String? UsuIng { get; set; }
        public DateTime? FecIng { get; set; }
        public String? UsuMod { get; set; }
        public DateTime? FecMod { get; set; }
        public Char? EstReg { get; set; }
    }
}