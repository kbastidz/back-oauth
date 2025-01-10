using api_gestion_escolar.TokenGenerator;
using Core.mapper;
using Core.service;
using db.Models;
//using edu.service;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Leer la configuración de UseToken desde appsettings.json
var useToken = builder.Configuration.GetValue<bool>("AppSettings:UseToken");

// Add services to the container.
builder.Services.AddDbContext<EduDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IAuthService, AuthService>();
//builder.Services.AddScoped<IEduService, EduService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddEndpointsApiExplorer();

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
        builder.WithOrigins("http://localhost:4200")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials());
});

// Agregar controladores
builder.Services.AddControllers(options =>
{
    if (useToken)
    {
        // Si UseToken es true, agregamos el filtro de validación de token
        options.Filters.Add<TokenValidationFilter>();
    }
});

// Configuración de Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "api_gestion_escolar", Version = "v1" });

    if (useToken)
    {
        // Si UseToken es true, agregar la definición de seguridad para Bearer token
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Por favor ingrese el token de tipo Bearer en el formato 'Bearer {token}'",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                },
                Array.Empty<string>()
            }
        });
    }
});

// Registrar el filtro TokenValidationFilter solo cuando sea necesario
if (useToken)
{
    builder.Services.AddScoped<TokenValidationFilter>(); // Asegúrate de registrar el filtro en el contenedor de dependencias
}

var app = builder.Build();

// Configurar el pipeline de la aplicación HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");

// Si UseToken es true, habilitar autenticación y autorización
if (useToken)
{
    app.UseAuthentication();
    app.UseAuthorization();
}

app.MapControllers();

app.Run();
