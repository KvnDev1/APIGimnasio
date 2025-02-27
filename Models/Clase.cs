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

        //Relacion: 1 Clase puede tener * Inscripciones
        public ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();

        //Relacion entre Clase y Profesor
        //Clave foranea de Profesor
        public Guid ProfesorId {get; set;}

        //Relacion: 1 Clase puede tener 1 Profesor
        public Profesor Profesor { get; set; } = null!;


    }
}