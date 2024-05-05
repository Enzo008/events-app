﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace events_app.Server.Models
{
    public class Usuario
    {
        [Key, Column(Order = 0)]
        public String? UsuAno { get; set; }
        [Key, Column(Order = 1)]
        public String? UsuCod { get; set; }
        [ForeignKey("DocumentoIdentidad")]
        public String? UsuNumDoc { get; set; }
        public String? UsuNom { get; set; }
        public String? UsuApe { get; set; }
        public String? UsuFecNac { get; set; }
        public String? UsuSex { get; set; }
        public String? UsuCorEle { get; set; }
        public String? UsuFecInc { get; set; }
        public String? UsuTel { get; set; }
        public String? UsuNomUsu { get; set; }
        public String? UsuPas { get; set; }
        public String? UsuEst { get; set; }
        public String? UsuIp { get; set; }
        [ForeignKey("Rol")]
        public String? RolCod { get; set; }
        public String? RolNom { get; set; }
        public byte[]? UsuAva { get; set; }
        public String? Ip { get; set; }
        public String? UsuIng { get; set; }
        public DateTime? FecIng { get; set; }
        public String? UsuMod { get; set; }
        public DateTime? FecMod { get; set; }
        public char EstReg { get; set; }
    }
}
