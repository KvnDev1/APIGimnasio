
using System.ComponentModel.DataAnnotations;

namespace APIGimnasio.Models.DTOs
{
    public class UsuarioDTO
    {
        public  Guid UsuarioId { get; set; }
        [Required]
        public string Nombre { get; set; } = string.Empty;
        [Required]
        public string Apellido { get; set; } = string.Empty;
        public string? Telefono { get; set; }

        // Lista de clases a las que está inscrito el usuario
        public List<ClaseDTO> ClasesInscritas { get; set; } = new List<ClaseDTO>();
    }
}