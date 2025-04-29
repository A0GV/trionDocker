var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Add connection string service
builder.Services.AddSingleton<string>(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    return configuration.GetConnectionString("DefaultConnection") ?? 
           "Server=construcciondesoftwate-databaselibroprueba.i.aivencloud.com;Port=15400;Database=oxxodb;Uid=avnadmin;Pwd=AVNS_EbD2wE2Jb0yXJYlPLsE;SslMode=Required;SslCa=ca.pem";
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "OxxoGame API",
        Version = "v1",
        Description = "API para el juego de Oxxo"
    });
});

var app = builder.Build();

// Siempre habilitar Swagger, sin importar el entorno
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "OxxoGame API v1");
    c.RoutePrefix = "swagger";
});

// Usar la política de CORS configurada
app.UseCors("AllowAllOrigins");

// Asegurar que UseRouting se llame antes de UseEndpoints
app.UseRouting();

app.UseAuthorization();

// Add a root endpoint that redirects to Swagger
app.MapGet("/", () => Results.Redirect("/swagger"));

// Configure controller routes with case insensitive matching
app.MapControllers();

app.Run();
