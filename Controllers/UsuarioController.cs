using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIGimnasio.Models;
using APIGimnasio.Models.DTOs;

namespace APIGimnasio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public UsuarioController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/usuario/MostrarUsuarios
        [HttpGet("MostrarUsuarios")]
        public async Task<ActionResult<IEnumerable<UsuarioDTO>>> MostrarUsuarios()
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.Inscripciones)  // Incluir inscripciones
                .ThenInclude(i => i.Clase)      // Incluir detalles de la clase
                .Select(u => new UsuarioDTO
                {
                    UsuarioId = u.UsuarioId,
                    Nombre = u.Nombre,
                    Apellido = u.Apellido,
                    Telefono = u.Telefono,

                    // Mapear las clases inscritas al UsuarioDTO
                    ClasesInscritas = u.Inscripciones
            .Where(i => i.Clase != null)  // Filtrar inscripciones sin clases asociadas
            .Select(i => new ClaseDTO
            {   // Aqui tenia error porque no estaba asegurando de que ClaseId no fuera Null
                ClaseId = i.Clase!.ClaseId, // Se soluciono con i.Clase! 
                Nombre = i.Clase.Nombre ?? "Clase no especificada",
                Descripcion = i.Clase.Descripcion ?? "Sin descripción",
                Horario = i.Clase.Horario ?? "Horario no especificado",
                CapacidadMaxima = i.Clase.CapacidadMaxima
            }).ToList()
                })
                .ToListAsync();

            return Ok(usuarios);
        }

        // GET: api/usuario/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDTO>> GetUsuarioById(Guid id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound("Usuario no encontrado");

            // Mapear a DTO
            var usuarioDto = new UsuarioDTO
            {
                UsuarioId = usuario.UsuarioId,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Telefono = usuario.Telefono
            };

            return Ok(usuarioDto);
        }

        // POST: api/usuario/CrearNuevoUsuario
        [HttpPost("CrearNuevoUsuario")]
        public async Task<ActionResult> CrearNuevoUsuario(UsuarioDTO Udto)
        {
            Usuario usuario = new Usuario
            {
                UsuarioId = Guid.NewGuid(),
                Nombre = Udto.Nombre,
                Apellido = Udto.Apellido,
                Telefono = Udto.Telefono
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return Ok("Usuario creado exitosamente");
        }

        // PUT: api/usuario/ActualizarUsuario/{id}
        [HttpPut("ActualizarUsuario/{id}")]
        public async Task<IActionResult> ActualizarUsuario(Guid id, UsuarioDTO usuarioDto)
        {
            if (id != usuarioDto.UsuarioId)
                return BadRequest("El ID del usuario no coincide");

            var usuarioExistente = await _context.Usuarios.FindAsync(id);
            if (usuarioExistente == null)
                return NotFound("Usuario no encontrado");

            // Actualizar los campos
            usuarioExistente.Nombre = usuarioDto.Nombre;
            usuarioExistente.Apellido = usuarioDto.Apellido;
            usuarioExistente.Telefono = usuarioDto.Telefono;

            _context.Entry(usuarioExistente).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok("Usuario actualizado exitosamente");
        }

        // DELETE: api/usuario/EliminarUsuario/{id}
        [HttpDelete("EliminarUsuario/{id}")]
        public async Task<IActionResult> EliminarUsuario(Guid id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound("Usuario no encontrado");

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return Ok("Usuario eliminado exitosamente");

        }
    }
}
