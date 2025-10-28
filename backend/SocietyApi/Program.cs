using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SocietyApi.Data;
using SocietyApi.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// DB - use env var first, else SQLite file
var conn = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection") ??
           configuration.GetConnectionString("DefaultConnection") ?? "Data Source=society.db";
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(conn));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var jwtKey = Environment.GetEnvironmentVariable("Jwt__Key") ?? configuration["Jwt:Key"] ?? "VerySecretKeyChangeThisInProduction";
var jwtIssuer = Environment.GetEnvironmentVariable("Jwt__Issuer") ?? configuration["Jwt:Issuer"] ?? "SocietyApi";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidIssuer = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// Ensure DB and seed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    // seed if empty
    if (!db.Users.Any())
    {
        var auth = scope.ServiceProvider.GetRequiredService<IAuthService>();
        // Run synchronously for startup seed
        auth.RegisterAsync("admin", "Admin123!", Models.Role.Admin, "Administrator", null, null).GetAwaiter().GetResult();
        auth.RegisterAsync("plumber1", "Plumber123!", Models.Role.Plumber).GetAwaiter().GetResult();
        auth.RegisterAsync("electric1", "Electric123!", Models.Role.Electrician).GetAwaiter().GetResult();
        auth.RegisterAsync("tenant1", "Tenant123!", Models.Role.Tenant, "Tenant One", "A-101", "+000").GetAwaiter().GetResult();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();