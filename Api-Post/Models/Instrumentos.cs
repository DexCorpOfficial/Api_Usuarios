using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json; // Asegúrate de incluir esta línea

namespace Api_Usuarios.Models
{
    public class Instrumentos
    {
        [Key, Column(Order = 0)]
        public int IDdeCuenta { get; set; }

        [Key, Column(Order = 1)]
        [StringLength(100)]
        public string Instrumento { get; set; }

        // Navegación a la entidad Cuenta, pero se ignora en la serialización
        [ForeignKey("IDdeCuenta")]
        [JsonIgnore] // Esta línea evita que la propiedad se incluya en el JSON
        public virtual Cuenta Cuenta { get; set; }
    }
}
