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
                .Select(i => new InscripcionDTO
                {
                    InscripcionId = i.InscripcionId,
                    UsuarioId = i.UsuarioId,
                    ClaseId = i.ClaseId,
                    FechaInscripcion = i.FechaInscripcion
                })
                .ToListAsync();

            return Ok(inscripciones);
        }

        
        [HttpGet("MostrarInscripcionesPor{id}")]
        public async Task<ActionResult<InscripcionDTO>> GetInscripcionById(Guid id)
        {
            var inscripcion = await _context.Inscripciones
                .Include(i => i.Usuario)
                .Include(i => i.Clase)
                .FirstOrDefaultAsync(i => i.InscripcionId == id);

            if (inscripcion == null)
                return NotFound("Inscripción no encontrada");

            return Ok(new InscripcionDTO
            {
                InscripcionId = inscripcion.InscripcionId,
                UsuarioId = inscripcion.UsuarioId,
                ClaseId = inscripcion.ClaseId,
                FechaInscripcion = inscripcion.FechaInscripcion
            });
        }


        [HttpPost("CrearNuevaInscripcion")]
        public async Task<ActionResult> CrearInscripcion(InscripcionDTO inscripcionDto)
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
                InscripcionId = Guid.NewGuid(),
                UsuarioId = inscripcionDto.UsuarioId,
                ClaseId = inscripcionDto.ClaseId,
                FechaInscripcion = DateTime.UtcNow
            };

            _context.Inscripciones.Add(nuevaInscripcion);
            await _context.SaveChangesAsync();

            return Ok("Inscripción creada exitosamente");
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
