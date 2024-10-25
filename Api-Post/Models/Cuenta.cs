using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api_Post.Models
{
    [Table("cuenta")]
    public class Cuenta
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Column("nombre")]
        public string Nombre { get; set; }

        [Column("foto_perfil")]
        public byte[] FotoPerfil { get; set; }

        [StringLength(200)]
        [Column("biografia")]
        public string Biografia { get; set; }

        [Required]
        [Column("seguidores")]
        public int Seguidores { get; set; } = 0;

        [Required]
        [StringLength(30)]
        [Column("genero")]
        public string Genero { get; set; }

        [Required]
        [Column("seguidos")]
        public int Seguidos { get; set; } = 0;

        [Required]
        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Required]
        [Column("fecha_nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        [Column("musico")]
        public bool Musico { get; set; } = false;

        [Required]
        [Column("activo")]
        public bool Activo { get; set; } = true;

        [Required]
        [StringLength(100)]
        [Column("contrasena")]
        public string Contrasena { get; set; }

        [Required]
        [Column("privado")]
        public bool Privado { get; set; } = true;
    }
}
