using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Configuracion para la conexion a la base de datos Postgres
builder.Services.AddDbContext<ApiDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//Habilitar los controladores
builder.Services.AddControllers();
//Configuracion de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "APIGimnasio", Version = "v1" });
});

var app = builder.Build();

// 4. Habilitar Swagger en modo desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "APIGimnasio v1"));
}

// 5. Middlewares básicos
app.UseHttpsRedirection();
app.UseAuthorization();

// 6. Mapear controladores automáticamente
app.MapControllers();

app.Run();
