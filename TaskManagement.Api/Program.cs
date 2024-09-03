using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TaskManagement.Api.Middlewares;
using TaskManagement.Api.Validators.UsersEndpoints;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Interfaces.Auth;
using TaskManagement.Domain.Interfaces.Repositories;
using TaskManagement.Domain.Interfaces.Services;
using TaskManagement.Infrastructure;
using TaskManagement.Infrastructure.Auth;
using TaskManagement.Infrastructure.Repositories;

namespace TaskManagement.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = "Authorization",
                        Type = ReferenceType.SecurityScheme
                    }
                };
                options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { securityScheme, new string[] { } }
                });
            });

            builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserRequestValidator>();
            builder.Services.AddFluentValidationAutoValidation();

            var configuration = builder.Configuration;

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["JwtOptions:SecretKey"])),
                    };
                });

            builder.Services.AddAuthorization();


            var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<ApiDbContext>(options =>
                    options.UseNpgsql(connectionString));

            builder.Services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));

            builder.Services.AddScoped<IJwtProvider, JwtProvider>();
            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

            builder.Services.AddScoped<IUsersRepository, UsersRepository>();

            builder.Services.AddScoped<IUsersService, UsersService>();

            builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
