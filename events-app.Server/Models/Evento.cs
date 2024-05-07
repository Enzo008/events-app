using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace events_app.Server.Models
{
    public class Evento
    {
        [Key, Column(Order = 0)]
        public String? EveAno { get; set; }

        [Key, Column(Order = 1)]
        public String? EveCod { get; set; }
        public String? EveFec { get; set; }
        public String? EveHor { get; set; }
        public String? EveNom { get; set; }
        public String? EveDes { get; set; }
        public String? EvePrePla { get; set; }
        public String? EvePreEje { get; set; }
        public String? UbiCod { get; set; }
        public String? UbiNom { get; set; }
        public String? UsuIng { get; set; }
        public DateTime? FecIng { get; set; }
        public String? UsuMod { get; set; }
        public DateTime? FecMod { get; set; }
        public Char? EstReg { get; set; }
    }
}