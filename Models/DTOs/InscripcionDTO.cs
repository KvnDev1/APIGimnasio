namespace APIGimnasio.Models.DTOs
{
    public class InscripcionDTO
    {
        public Guid InscripcionId { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid ClaseId { get; set; }

        /*Aqui me habia tirado un error con la FechaInscripcion por el tema de que Postgres maneja las 
        fechas en UTC entonces cambie DateTime.Now por DateTime.UtcNow.*/
        public DateTime FechaInscripcion { get; set; } = DateTime.UtcNow;
    }
}