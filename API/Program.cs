using System.Text;
using System.Text.Json.Serialization;
using Core.Enities.ProjectAggregate;
using Core.Interfaces;
using Core.Services;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowLocalhost4200",
                builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });

        builder.Services.AddDbContext<TranslationManagementDbContext>(options =>
            options.UseSqlServer("Server=tcp:mtaweelsqlserver.database.windows.net,1433;Initial Catalog=TranslationManagmenetSystem;Persist Security Info=False;User ID=mmaltaweel;Password=BsdW@dsasd182!@#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60"));

        // For Identity
        builder.Services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<TranslationManagementDbContext>()
            .AddDefaultTokenProviders();

        // Adding Authentication
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            // Adding Jwt Bearer
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
                };
            });
        // Register your repositories
        builder.Services.AddScoped<IAsyncRepository<Project>, EfRepository<Project>>();
        builder.Services.AddScoped<IAsyncRepository<ProjectTask>, EfRepository<ProjectTask>>();
        builder.Services.AddScoped<IAsyncRepository<User>, EfRepository<User>>();

        // Register your services
        builder.Services.AddScoped<IProjectService, ProjectService>();
        builder.Services.AddScoped<IReportService, ReportService>();
        builder.Services.AddScoped<IProjectTaskService, ProjectTaskService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddSingleton<ILog>(new LoggerFake());
        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.WriteIndented = true;
        });
        builder.Services.AddEndpointsApiExplorer();
        //builder.Services.AddSwaggerGen();
        builder.Services.AddSwaggerGen(c =>
        {
            // Add the security definition for bearer tokens
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            // Enable authentication globally for Swagger UI
            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        });
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCors("AllowLocalhost4200"); // Use the CORS policy

        app.MapControllers();

        app.Run();
    }
}