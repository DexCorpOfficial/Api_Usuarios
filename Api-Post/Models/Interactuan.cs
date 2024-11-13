using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api_Usuarios.Models
{
    public class Interactuan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Configura ID como autoincremental
        public int ID { get; set; }

        [JsonProperty("IDdeEmisor")]
        [Required]
        public int IDdeEmisor { get; set; }

        [JsonProperty("IDdeReceptor")]
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

        // Propiedades de navegación para las relaciones con Cuenta
        public virtual Cuenta Emisor { get; set; }
        public virtual Cuenta Receptor { get; set; }
    }
}
