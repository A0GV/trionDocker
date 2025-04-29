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
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // In production, still make Swagger available
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Usar la política de CORS configurada
app.UseCors("AllowAllOrigins");

app.UseAuthorization();

// Add a root endpoint that redirects to Swagger
app.MapGet("/", () => Results.Redirect("/swagger"));

// Configure controller routes with case insensitive matching
app.MapControllers();

app.Run();
