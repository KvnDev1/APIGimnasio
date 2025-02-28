using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGimnasio.Models.DTOs
{
    public class InscripcionCreateDTO
    {
        public Guid UsuarioId { get; set; }
        public Guid ClaseId { get; set; }
    }
}