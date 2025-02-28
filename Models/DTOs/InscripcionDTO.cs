namespace APIGimnasio.Models.DTOs
{
    public class InscripcionDTO
    {
        public Guid InscripcionId { get; set; }
        public string UsuarioNombre { get; set; } = string.Empty; //Agregado para que muestre el nombre del usuario(GET & GET/{id})
        public string ClaseNombre { get; set; } = string.Empty; //Agregado para que muestre el nombre de la clase(GET & GET/{id})
        public string ProfesorNombre { get; set; } = string.Empty; //Agregado para que muestre el nombre del profesor(GET & GET/{id})

        /*Aqui me habia tirado un error con la FechaInscripcion por el tema de que Postgres maneja las 
        fechas en UTC entonces cambie DateTime.Now por DateTime.UtcNow.*/
        public DateTime FechaInscripcion { get; set; } = DateTime.UtcNow;
    }
}