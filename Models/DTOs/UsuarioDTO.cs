// Purpose: DTO para la entidad Usuario.
// Este DTO se utiliza para mostrar los detalles de un usuario, incluyendo las clases a las que está inscrito.

namespace APIGimnasio.Models.DTOs
{
    public class UsuarioDTO
    {
        public  Guid UsuarioId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string? Telefono { get; set; }

        // Lista de clases a las que está inscrito el usuario
        public List<ClaseDTO> ClasesInscritas { get; set; } = new List<ClaseDTO>(); //Esto mostrara el detalle de las clases inscritas
    }
}