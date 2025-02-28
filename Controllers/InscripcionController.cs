using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIGimnasio.Models;
using APIGimnasio.Models.DTOs;

namespace APIGimnasio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InscripcionController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public InscripcionController(ApiDbContext context)
        {
            _context = context;
        }

        
        [HttpGet("MostrarInscripciones")]
        public async Task<ActionResult<IEnumerable<InscripcionDTO>>> GetInscripciones()
        {
            var inscripciones = await _context.Inscripciones
                .Include(i => i.Usuario)  // Incluir detalles del usuario
                .Include(i => i.Clase)    // Incluir detalles de la clase
                .ThenInclude(c => c.Profesor) // Incluir detalles del profesor
                .Select(i => new InscripcionDTO
                {
                    InscripcionId = i.InscripcionId,
                    UsuarioNombre = i.Usuario != null ? $"{i.Usuario.Nombre} {i.Usuario.Apellido}" : "Usuario no especificado",
                    ClaseNombre = i.Clase != null ? i.Clase.Nombre : "Clase no especificada",
                    ProfesorNombre = i.Clase != null && i.Clase.Profesor != null ? $"{i.Clase.Profesor.Nombre} {i.Clase.Profesor.Apellido}" : "Profesor no especificado",
                    FechaInscripcion = i.FechaInscripcion,
                })
                .ToListAsync();

            return Ok(inscripciones);
        }

        
        [HttpGet("MostrarInscripcionesPor{id}")]
        public async Task<ActionResult<InscripcionDTO>> GetInscripcionById(Guid id)
        {
            var inscripciones = await _context.Inscripciones
                .Include(i => i.Usuario) // Incluir detalles del usuario
                .Include(i => i.Clase) // Incluir detalles de la clase
                .ThenInclude(c => c.Profesor) // Incluir detalles del profesor
                .FirstOrDefaultAsync(i => i.InscripcionId == id);

            if (inscripciones == null)
                return NotFound("Inscripción no encontrada");

            return Ok(new InscripcionDTO
            {
                InscripcionId = inscripciones.InscripcionId,
                UsuarioNombre = inscripciones.Usuario != null ? $"{inscripciones.Usuario.Nombre} {inscripciones.Usuario.Apellido}" : "Usuario no especificado",
                ClaseNombre = inscripciones.Clase != null ? inscripciones.Clase.Nombre : "Clase no especificada",
                ProfesorNombre = inscripciones.Clase != null && inscripciones.Clase.Profesor != null ? $"{inscripciones.Clase.Profesor.Nombre} {inscripciones.Clase.Profesor.Apellido}" : "Profesor no especificado",
                FechaInscripcion = inscripciones.FechaInscripcion,
            });
        }


        [HttpPost("CrearNuevaInscripcion")]
        public async Task<ActionResult> CrearInscripcion([FromBody]InscripcionCreateDTO inscripcionDto)
        {
            var usuario = await _context.Usuarios.FindAsync(inscripcionDto.UsuarioId);
            var clase = await _context.Clases.Include(c => c.Inscripciones).FirstOrDefaultAsync(c => c.ClaseId == inscripcionDto.ClaseId);

            if (usuario == null)
                return NotFound("Usuario no encontrado");

            if (clase == null)
                return NotFound("Clase no encontrada");

            // Verificar si el usuario ya está inscrito en la clase
            var inscripcionExistente = await _context.Inscripciones
                .FirstOrDefaultAsync(i => i.UsuarioId == inscripcionDto.UsuarioId && i.ClaseId == inscripcionDto.ClaseId);

            if (inscripcionExistente != null)
                return BadRequest("El usuario ya está inscrito en esta clase");

            // Validar la capacidad máxima de la clase
            if (clase.Inscripciones.Count >= clase.CapacidadMaxima)
                return BadRequest("La clase ya ha alcanzado su capacidad máxima");

            // Crear la inscripción
            Inscripcion nuevaInscripcion = new Inscripcion
            {
                UsuarioId = inscripcionDto.UsuarioId,
                ClaseId = inscripcionDto.ClaseId,
                FechaInscripcion = DateTime.UtcNow
            };

            _context.Inscripciones.Add(nuevaInscripcion);
            await _context.SaveChangesAsync();

            return Ok("Inscripción creada exitosamente con la ID: " + nuevaInscripcion.InscripcionId);
        }

        // DELETE: api/inscripcion/{id}
        [HttpDelete("EliminarInscripcion{id}")]
        public async Task<IActionResult> EliminarInscripcion(Guid id)
        {
            var inscripcion = await _context.Inscripciones.FindAsync(id);
            if (inscripcion == null)
                return NotFound("Inscripción no encontrada");

            _context.Inscripciones.Remove(inscripcion);
            await _context.SaveChangesAsync();
            return Ok("Inscripción eliminada exitosamente");
        }
    }
}
