

namespace APIGimnasio.Models.DTOs
{
    public class ProfesorDTO
    {
        public Guid ProfesorId { get; set; }
        public string Nombre { get; set;} = string.Empty;
        public string Apellido { get; set;} = string.Empty;
    }
}