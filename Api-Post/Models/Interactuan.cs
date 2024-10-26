using System;
using System.ComponentModel.DataAnnotations;

namespace Api_Usuarios.Models
{
    public class Interactuan
    {
        [Required]
        public int IDdeEmisor { get; set; }

        [Required]
        public int IDdeReceptor { get; set; }

        [Required]
        [StringLength(50)]
        public string Tipo { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public string Notificacion { get; set; }

        public string Estado { get; set; }
        public string Contenido { get; set; }
        public bool? Seguido { get; set; } 
        public virtual Cuenta Emisor { get; set; }
        public virtual Cuenta Receptor { get; set; }
    }
}
