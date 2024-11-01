using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Api_Usuarios.Models
{
    public class Instrumentos
    {
        [Key, Column(Order = 0)]
        public int IDdeCuenta { get; set; }

        [Key, Column(Order = 1)]
        [StringLength(100)]
        public string Instrumento { get; set; }

        // Navegación a la entidad Cuenta
        [ForeignKey("IDdeCuenta")]
        public virtual Cuenta Cuenta { get; set; }
    }
}
