
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProjetMaisonCo_backend.Entities.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MyDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("Main")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer( o =>
{
    o.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration.GetSection("TokenInfo").GetSection("secret").Value!)),
        ValidateIssuer = false,
        ValidIssuer = builder.Configuration.GetSection("TokenInfo").GetSection("issuer").Value!,
        ValidAudience = builder.Configuration.GetSection("TokenInfo").GetSection("audience").Value!,
        ValidateLifetime = false
    };
});

//Mise en place des politiques
builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("Auth", p => p.RequireAuthenticatedUser());
    o.AddPolicy("Admin", p => p.RequireRole("Admin"));
});

builder.Services.AddCors(o =>
{
    o.AddDefaultPolicy(p =>
    { //adapter le lien localhost au projet!!
        p.WithOrigins("http://localhost:4200", builder.Configuration.GetSection("TokenInfo").GetSection("audience").Value!);
        p.AllowAnyHeader();
        p.AllowAnyMethod();
    });
});

// Pour passer du JWT avec SWAGGER
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyProject", Version = "v1.0.0" });
    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    c.AddSecurityDefinition("Bearer", securitySchema);
    var securityRequirement = new OpenApiSecurityRequirement
                 {
                     { securitySchema, new[] { "Bearer" } }
                 };
    c.AddSecurityRequirement(securityRequirement);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseCors();

app.MapControllers();

app.Run();
