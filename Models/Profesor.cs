
using System.ComponentModel.DataAnnotations;


namespace APIGimnasio.Models
{
    public class Profesor
    {
        [Key]
        public Guid ProfesorId { get; set; }
        [Required]
        public string Nombre { get; set; } = string.Empty;
        [Required]
        public string Apellido { get; set; } = string.Empty ;

        //Relacion entre Profesor y Clase 'Un profesor puede estar asignado a muchas clases'
        public ICollection<Clase> Clases { get; set; } = new List<Clase>();



    }
}