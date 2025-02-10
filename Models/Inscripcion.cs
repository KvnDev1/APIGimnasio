using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGimnasio.Models
{
    public class Inscripcion
    {
        [Key]
        public Guid InscripcionId { get; set; }

        [ForeignKey("Usuario")]
        public Guid UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;
        	

        [ForeignKey("Clase")]
        public Guid ClaseId { get; set; }
        public Clase Clase { get; set; } = null!;
        

        // DateTime.Utc.Now asegura que la fecha se guarde como UTC y no lance un error en Postgres
        public DateTime FechaInscripcion { get; set; } = DateTime.UtcNow;
    }
}

        

    