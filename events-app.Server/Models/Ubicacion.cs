using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace events_app.Server.Models
{
    public class Ubicacion
    {
        [Key, Column(Order = 0)]
        public String? UbiCod { get; set; }
        public String? UbiNom { get; set; }
        public String? UbiDir { get; set; }
        public String? UbiCapPer { get; set; }
        public String? UsuIng { get; set; }
        public DateTime? FecIng { get; set; }
        public String? UsuMod { get; set; }
        public DateTime? FecMod { get; set; }
        public Char? EstReg { get; set; }
    }
}