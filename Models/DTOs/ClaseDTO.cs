
using System.ComponentModel.DataAnnotations;

namespace APIGimnasio.Models.DTOs
{
    public class ClaseDTO
    {
        public Guid ClaseId { get; set; }
        [Required]
        public string Nombre { get; set; } = string.Empty;
        [Required]
        public string Descripcion {get; set;} = string.Empty;
        [Required]
        public string Horario {get; set;} = string.Empty;
        [Required]
        public int CapacidadMaxima {get; set;}
        public Guid ProfesorId  { get; set;} //ID del Profesor
        public string ProfesorNombre {get; set;} = string.Empty; //Nombre del profesor
    }
}