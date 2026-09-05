using System.Text;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pegasus.API.Middleware;
using Pegasus.Core.Interfaces;
using Pegasus.Infrastructure.Data;
using Pegasus.Infrastructure.Repositories;
using Pegasus.Infrastructure.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Cargar variables de entorno desde el archivo .env
Env.Load();

// 2. Obtener cadena de conexión
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");

// 3. Registrar DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString,
        b => b.MigrationsAssembly("Pegasus.Infrastructure")));

// 4. Inyección de dependencias
builder.Services.AddScoped<IVehiculoRepository, VehiculoRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

// 5. Configurar Autenticación JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                Environment.GetEnvironmentVariable("JWT_SECRET") ?? "SuperSecretKeyPegasus2026_MustBeLongEnough!")),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Cookies["jwt"];
                if (!string.IsNullOrEmpty(token))
                {
                    context.Token = token;
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// 6. Configuración CORS con tu IP local
builder.Services.AddCors(options =>
{
    options.AddPolicy("PoliticaPegasus", policy =>
    {
        policy.WithOrigins(
            "http://localhost:777",
            "http://192.168.1.126:777",
            "http://localhost:5173",
            "http://192.168.1.126:5173"
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

var app = builder.Build();

// 7. Middlewares
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCors("PoliticaPegasus");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();