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
                .Select(c => new ClaseDTO
                {
                    ClaseId = c.ClaseId,
                    Nombre = c.Nombre,
                    Descripcion = c.Descripcion,
                    Horario = c.Horario,
                    CapacidadMaxima = c.Inscripciones.Count() // Obtener la capacidad actual desde las inscripciones
                })
                .ToListAsync();

            return Ok(clases);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ClaseDTO>> GetClaseById(Guid id)
        {
            var clase = await _context.Clases
                .Include(c => c.Inscripciones)  // Incluir las inscripciones si es necesario
                .FirstOrDefaultAsync(c => c.ClaseId == id);

            if (clase == null)
                return NotFound("Clase no encontrada");

            return Ok(new ClaseDTO
            {
                ClaseId = clase.ClaseId,
                Nombre = clase.Nombre,
                Descripcion = clase.Descripcion,
                Horario = clase.Horario,
                CapacidadMaxima = clase.Inscripciones.Count()
            });
        }

        [HttpPost("CrearNuevaClase")]
        public async Task<ActionResult> CrearClase(ClaseDTO claseDto)
        {
            Clase nuevaClase = new Clase
            {
                ClaseId = Guid.NewGuid(),
                Nombre = claseDto.Nombre,
                Descripcion = claseDto.Descripcion,
                Horario = claseDto.Horario,
                CapacidadMaxima = claseDto.CapacidadMaxima,
                Inscripciones = new List<Inscripcion>() // Se inicializa vacío
            };

            _context.Clases.Add(nuevaClase);
            await _context.SaveChangesAsync();
            return Ok("Clase creada exitosamente");
        }

        [HttpPut("ActualizarClase{id}")]
        public async Task<IActionResult> ActualizarClase(Guid id, ClaseDTO claseDto)
        {
            if (id != claseDto.ClaseId)
                return BadRequest("El ID de la clase no coincide");

            var claseExistente = await _context.Clases.FindAsync(id);
            if (claseExistente == null)
                return NotFound("Clase no encontrada");

            // Actualizar los campos
            claseExistente.Nombre = claseDto.Nombre;
            claseExistente.Descripcion = claseDto.Descripcion;
            claseExistente.Horario = claseDto.Horario;
            claseExistente.CapacidadMaxima = claseDto.CapacidadMaxima;

            _context.Entry(claseExistente).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok("Clase actualizada exitosamente");
        }

        [HttpDelete("EliminarClase{id}")]
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
