using SanDiaryApi.Data;
using SanDiaryApi.Services;
using SanDiaryApi.Models;

using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<NoteService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Введите JWT токен"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
            "http://localhost:5173",
            "http://192.168.0.102:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var authService = scope.ServiceProvider.GetRequiredService<AuthService>();

    db.Database.Migrate();

    if (!db.Users.Any())
    {
        var adminEmail = "admin@admin.admin";
        var adminPassword = "admin";

        var admin = new User
        {
            Email = adminEmail,
            Role = "Admin"
        };

        var passwordHasher = new PasswordHasher<User>();
        admin.PasswordHash = passwordHasher.HashPassword(admin, adminPassword);

        db.Users.Add(admin);
        db.SaveChanges();

        var token = authService.CreateToken(admin);

        app.Logger.LogInformation(
@"========================================
DEV ADMIN CREATED
Email: admin@local.dev
JWT TOKEN (use in Swagger):
{Token}
========================================", token);
    }

}

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowFrontend");

app.MapControllers();
app.Run();
