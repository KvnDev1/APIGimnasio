using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIGimnasio.Models;
using APIGimnasio.Models.DTOs;

namespace APIGimnasio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfesorController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public ProfesorController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpGet("MostrarProfesores")]
        public async Task<ActionResult<IEnumerable<ProfesorDTO>>> GetProfesores()
        {
            var profesores = await _context.Profesores
                .Select(p => new ProfesorDTO
                {
                    ProfesorId = p.ProfesorId,
                    Nombre = p.Nombre,
                    Apellido = p.Apellido
                })
                .ToListAsync();

            return Ok(profesores);
        }

        [HttpGet("MostrarProfesoresPor{id}")]
        public async Task<ActionResult<ProfesorDTO>> GetProfesorById(Guid id)
        {
            var profesor = await _context.Profesores.FindAsync(id);
            if (profesor == null)
                return NotFound("Profesor no encontrado");

            var profesorDto = new ProfesorDTO
            {
                ProfesorId = profesor.ProfesorId,
                Nombre = profesor.Nombre,
                Apellido = profesor.Apellido
            };

            return Ok(profesorDto);
        }

        [HttpPost("CrearNuevoProfesor")]
        public async Task<ActionResult> CrearProfesor(ProfesorDTO profesorDto)
        {
            var profesor = new Profesor
            {
                ProfesorId = Guid.NewGuid(),
                Nombre = profesorDto.Nombre,
                Apellido = profesorDto.Apellido
            };

            _context.Profesores.Add(profesor);
            await _context.SaveChangesAsync();

            return Ok("Profesor creado exitosamente");
        }

        [HttpPut("ActualizarProfesor{id}")]
        public async Task<ActionResult> ActualizarProfesor(Guid id, ProfesorDTO profesorDto)
        {
            if (id != profesorDto.ProfesorId)
                return BadRequest("El ID del profesor no coincide");

            var profesorExistente = await _context.Profesores.FindAsync(id);
            if (profesorExistente == null)
                return NotFound("Profesor no encontrado");

            profesorExistente.Nombre = profesorDto.Nombre;
            profesorExistente.Apellido = profesorDto.Apellido;

            _context.Entry(profesorExistente).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok("Profesor actualizado exitosamente");
        }

        [HttpDelete("EliminarProfesor{id}")]
        public async Task<ActionResult> EliminarProfesor(Guid id)
        {
            var profesor = await _context.Profesores.FindAsync(id);
            if (profesor == null)
                return NotFound("Profesor no encontrado");

            _context.Profesores.Remove(profesor);
            await _context.SaveChangesAsync();

            return Ok("Profesor eliminado exitosamente");
        }
    }
}