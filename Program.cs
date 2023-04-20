using System;
using System.IO;
using TeczkaCore.Models.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using MySql.Data.MySqlClient;
using TeczkaCore.Models.Interfaces;
using TeczkaCore.Models;
using TeczkaCore.Identity;
using Microsoft.AspNetCore.Identity;
using TeczkaCore.Controllers;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using TeczkaCore.Services;
using TeczkaCore.Entities;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo() { Title = "TeczkaCore", Version = "v1" });
  c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
  {
    Name = "Authorization",
    Type = SecuritySchemeType.ApiKey,
    Scheme = "Bearer",
    BearerFormat = "JWT",
    In = ParameterLocation.Header,
    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
  });
  c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddDbContext<TeczkaCoreContext>(
#if DEVIL
  option => option.UseMySQL(builder.Configuration.GetConnectionString("MyDevilConnectionString"))
#endif
#if ZZS
  option => option.UseSqlServer(builder.Configuration.GetConnectionString("ZzsConnectionString"))
#endif
#if WEBIO
  option => option.UseMySQL(builder.Configuration.GetConnectionString("WebioConnectionString"))
#endif
#if AWH
  option => option.UseMySQL(builder.Configuration.GetConnectionString("AWHConnectionString"))
#endif
#if MOTION
  option => option.UseMySQL(builder.Configuration.GetConnectionString("InMotionConnectionString"))
#endif
);

// Add services to the container.
var jwtOptions = new JwtOptions();
builder.Configuration.GetSection("jwt").Bind(jwtOptions);
builder.Services.AddSingleton(jwtOptions);

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(cfg =>
{
    cfg.SaveToken = true;
    cfg.RequireHttpsMetadata = false;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = builder.Configuration["jwt:JwtAudience"],
        ValidIssuer = builder.Configuration["jwt:JwtIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:JwtKey"]))
    };
});
builder.Services.AddAuthorization(options =>
{
  //options.AddPolicy("HasNationality", builder => builder.RequireClaim("Nationality", "German", "English"));
  //options.AddPolicy("AtLeast18", builder => builder.AddRequirements(new MinimumAgeRequirement(18)));
});


builder.Services.AddControllers();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddTransient<SeedRole>();
builder.Services.AddTransient<SeedUser>();
builder.Services.AddTransient<SeedArticle>();
builder.Services.AddTransient<SeedSection>();
builder.Services.AddTransient<SeedScan>();
builder.Services.AddTransient<SeedPerson>();
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddScoped<TeczkaCoreSeeder>();
builder.Services.AddSpaStaticFiles(configuration =>
{
  configuration.RootPath = "TeczkaFront/dist";
});

WebApplication app = builder.Build();
using (var scope = app.Services.CreateScope())
{
  var services = scope.ServiceProvider;
  var seederService = services.GetService<TeczkaCoreSeeder>();
  seederService?.SetApplication(app);
  seederService?.SeedRoles();
  seederService?.SeedUsers();
  seederService?.SeedArticles();
  seederService?.SeedSections();
  seederService?.SeedScans();
  seederService?.SeedPersons();

}

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
  app.UseStaticFiles();
  app.UseStaticFiles(new StaticFileOptions()
  {
    FileProvider = new PhysicalFileProvider(
      Path.Combine(Directory.GetCurrentDirectory(), @"StaticFile")),
    RequestPath = new PathString("/StaticFile")
  });

}
else
{
  app.UseDefaultFiles();
  app.UseStaticFiles();
  app.UseSpaStaticFiles();
}

//app.UseHttpsRedirection();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
