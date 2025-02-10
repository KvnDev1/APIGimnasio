using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace APIGimnasio.Models
{
    public class Usuario
    {
        [Key]
        public Guid UsuarioId { get; set; }
        [Required]
        public string Nombre { get; set; } = string.Empty;
        [Required]
        public string Apellido { get; set; } = string.Empty;
        public string? Telefono { get; set; }

        // Relación: Un usuario puede tener varias inscripciones
        public ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();
    }
}
