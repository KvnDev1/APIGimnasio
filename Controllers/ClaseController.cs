using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIGimnasio.Models;
using APIGimnasio.Models.DTOs;

namespace APIGimnasio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClaseController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public ClaseController(ApiDbContext context)
        {
            _context = context;
        }


        [HttpGet("MostrarClases")]
public async Task<ActionResult<IEnumerable<ClaseDTO>>> GetClases()
{
    var clases = await _context.Clases
        .Include(c => c.Profesor) // Incluir información del profesor
        .Select(c => new ClaseDTO
        {
            ClaseId = c.ClaseId,
            Nombre = c.Nombre,
            Descripcion = c.Descripcion,
            Horario = c.Horario,
            CapacidadMaxima = c.CapacidadMaxima,
            ProfesorId = c.ProfesorId,
            ProfesorNombre = c.Profesor != null ? $"{c.Profesor.Nombre} {c.Profesor.Apellido}" : "Sin profesor asignado",
        })
        .ToListAsync();

    return Ok(clases);
}


        [HttpGet("{id}")]
        public async Task<ActionResult<ClaseDTO>> GetClaseById(Guid id)
        {
            var clase = await _context.Clases
                .Include(c => c.Profesor) // Incluir información del profesor
                .FirstOrDefaultAsync(c => c.ClaseId == id);

            if (clase == null)
                return NotFound("Clase no encontrada");

            var claseDto = new ClaseDTO
            {
                ClaseId = clase.ClaseId,
                Nombre = clase.Nombre,
                Descripcion = clase.Descripcion,
                Horario = clase.Horario,
                CapacidadMaxima = clase.CapacidadMaxima,
                ProfesorId = clase.ProfesorId,
                ProfesorNombre = clase.Profesor != null ? $"{clase.Profesor.Nombre} {clase.Profesor.Apellido}" : "Sin profesor asignado"
            };

            return Ok(claseDto);
        }


        [HttpPost("CrearNuevaClase")]
        public async Task<ActionResult> CrearClase(ClaseDTO claseDto)
        {
            // Verificar que el profesor exista
            var profesor = await _context.Profesores.FindAsync(claseDto.ProfesorId);
            if (profesor == null)
                return BadRequest("El profesor especificado no existe");

            var nuevaClase = new Clase
            {
                ClaseId = Guid.NewGuid(),
                Nombre = claseDto.Nombre,
                Descripcion = claseDto.Descripcion,
                Horario = claseDto.Horario,
                CapacidadMaxima = claseDto.CapacidadMaxima,
                ProfesorId = claseDto.ProfesorId // Asignar el profesor
            };

            _context.Clases.Add(nuevaClase);
            await _context.SaveChangesAsync();

            return Ok("Clase creada exitosamente");
        }


        [HttpPut("ActualizarClase/{id}")]
        public async Task<IActionResult> ActualizarClase(Guid id, ClaseDTO claseDto)
        {
            if (id != claseDto.ClaseId)
                return BadRequest("El ID de la clase no coincide");

            var claseExistente = await _context.Clases.FindAsync(id);
            if (claseExistente == null)
                return NotFound("Clase no encontrada");

            // Verificar que el profesor exista
            var profesor = await _context.Profesores.FindAsync(claseDto.ProfesorId);
            if (profesor == null)
                return BadRequest("El profesor especificado no existe");

            // Actualizar los campos
            claseExistente.Nombre = claseDto.Nombre;
            claseExistente.Descripcion = claseDto.Descripcion;
            claseExistente.Horario = claseDto.Horario;
            claseExistente.CapacidadMaxima = claseDto.CapacidadMaxima;
            claseExistente.ProfesorId = claseDto.ProfesorId; // Actualizar el profesor

            _context.Entry(claseExistente).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok("Clase actualizada exitosamente");
        }


        [HttpDelete("EliminarClase/{id}")]
        public async Task<IActionResult> EliminarClase(Guid id)
        {
            var clase = await _context.Clases.FindAsync(id);
            if (clase == null)
                return NotFound("Clase no encontrada");

            _context.Clases.Remove(clase);
            await _context.SaveChangesAsync();

            return Ok("Clase eliminada exitosamente");
        }
    }
}