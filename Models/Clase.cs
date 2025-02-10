using System.ComponentModel.DataAnnotations;

namespace APIGimnasio.Models
{
    public class Clase
    {
        [Key]
        public Guid ClaseId { get; set; }
        [Required]
        public string Nombre { get; set; } = string.Empty;
        [Required]
        public string Descripcion {get; set;} = string.Empty;
        [Required]
        public string Horario {get; set;} = string.Empty;
        [Required]
        public int CapacidadMaxima {get; set;}

        //Aqui van las relaciones, en este caso la relacion entre Clase e Inscripcion
        public ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();
    }
}