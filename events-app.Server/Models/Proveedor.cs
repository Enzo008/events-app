using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace events_app.Server.Models
{
    public class Proveedor
    {
        [Key, Column(Order = 0)]
        public String? ProAno { get; set; }
        [Key, Column(Order = 1)]
        public String? ProCod { get; set; }
        public String? ProNom { get; set; }
        public String? ProApe { get; set; }
        public String? ProTel { get; set; }
        public String? ProSerCos { get; set; }
        public String? SerCan { get; set; }
        public String? UsuIng { get; set; }
        public DateTime? FecIng { get; set; }
        public String? UsuMod { get; set; }
        public DateTime? FecMod { get; set; }
        public Char? EstReg { get; set; }
    }
}