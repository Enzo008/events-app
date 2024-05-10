using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace events_app.Server.Models
{
    public class Material
    {
        [Key, Column(Order = 0)]
        public String? MatCod { get; set; }
        public String? MatNom { get; set; }
        public String? MatDes { get; set; }
        public String? MatPre { get; set; }
        public String? EveMatCan { get; set; }
        public String? UsuIng { get; set; }
        public DateTime? FecIng { get; set; }
        public String? UsuMod { get; set; }
        public DateTime? FecMod { get; set; }
        public Char? EstReg { get; set; }
    }
}