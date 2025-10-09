using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using EFCore.NamingConventions;
using PlantAnimalApi.Data;
using PlantAnimalApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Helper for postgres connection strings
static string ToNpgsql(string url)
{
    if (url.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase) ||
        url.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase))
    {
        var uri = new Uri(url.Replace("postgresql://", "postgres://", StringComparison.OrdinalIgnoreCase));
        var userInfo = uri.UserInfo.Split(':', 2);
        var user = Uri.UnescapeDataString(userInfo[0]);
        var pass = Uri.UnescapeDataString(userInfo[1]);
        var host = uri.Host;
        var port = uri.IsDefaultPort ? 5432 : uri.Port;
        var db   = uri.AbsolutePath.Trim('/');
        return $"Host={host};Port={port};Database={db};Username={user};Password={pass};Ssl Mode=Require;Trust Server Certificate=true;Pooling=true;";
    }
    return url; // already in Npgsql format
}


DotNetEnv.Env.Load();
// ---- DB connection ----
// Prefer env var DATABASE_URL in prod (Render/Azure), fall back to appsettings connstring in dev.
var raw = Environment.GetEnvironmentVariable("DATABASE_URL")
        ?? throw new InvalidOperationException("Missing DB connection string.");

Console.WriteLine($"Connection string: {raw}");

builder.Services.AddDbContext<BioscopeDbContext>(opt =>
    opt.UseNpgsql(ToNpgsql(raw)).UseSnakeCaseNamingConvention());

// ---- Auth (JWT) ----
var jwtKey = builder.Configuration["JWT:Key"]
            ?? Environment.GetEnvironmentVariable("JWT__Key")
            ?? "dev-super-secret";
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.FromMinutes(2)
        };
    });

builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddSingleton<OrganismService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ---- Auto-migrate at startup (applies pending EF migrations) ----
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BioscopeDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
