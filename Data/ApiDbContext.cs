using APIGimnasio.Models;
using Microsoft.EntityFrameworkCore;

public class ApiDbContext : DbContext
{
    // DbSets para mapear los modelos y tablas en la BD
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Clase> Clases { get; set; }
    public DbSet<Inscripcion> Inscripciones { get; set; }

    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        /*Aqui tenia agregada la configuacion de la PK de Usuario pero como ya lo tengo con Key
        no era necesario agregarlo aqui tambien ya que EF lo detecta automaticamente.*/


        // Configuración de relaciones
        modelBuilder.Entity<Inscripcion>()
            .HasOne(i => i.Usuario)
            .WithMany(u => u.Inscripciones)  // Un usuario puede tener muchas inscripciones
            .HasForeignKey(i => i.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);  // Si se elimina el usuario, se eliminan sus inscripciones

        modelBuilder.Entity<Inscripcion>()
            .HasOne(i => i.Clase)
            .WithMany(c => c.Inscripciones)  // Una clase puede tener muchas inscripciones
            .HasForeignKey(i => i.ClaseId)
            .OnDelete(DeleteBehavior.Cascade);  // Si se elimina la clase, se eliminan las inscripciones
    }
}
