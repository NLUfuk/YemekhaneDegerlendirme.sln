// Program.cs (Yemekhane.API)

using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


using Yemekhane.Business.Services.Implementations;
using Yemekhane.Business.Services.Interfaces;
using Yemekhane.Data;
using Yemekhane.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile(new MealProfile());
    cfg.AddProfile(new MealSuggestionProfile());
    cfg.AddProfile(new RatingProfile());
    cfg.AddProfile(new UserProfile());
});    
  


//  Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//  DbContext (appsettings.json -> ConnectionStrings:DefaultConnection)
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//  Repository'ler (tek namespace: Yemekhane.Data.Repositories)
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMealRepository, MealRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();  
builder.Services.AddScoped<ISuggestionRepository, SuggestionRepository>();

//  Servisler
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMealService, MealService>();
builder.Services.AddScoped<ISuggestionService, SuggestionService>();

//  JWT Authentication (appsettings.json -> Jwt:Key)
var jwtKey = builder.Configuration["Jwt:Key"] ?? "DEV-KEY-CHANGE-ME";
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;      // dev i�in
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Yemekhane API", Version = "v1" });

    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Token'� �u formatta gir: Bearer {token}",
        Reference = new OpenApiReference
        {
            Id = "Bearer",
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition("Bearer", jwtSecurityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

var app = builder.Build();

//#if DEBUG
//var mapper = app.Services.GetRequiredService<IMapper>();
//mapper.ConfigurationProvider.AssertConfigurationIsValid(); // sorun varsa burada yazar
//#endif

// use SeedData.Initialize(app.Services); // veritaban�n� seed et
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (dbContext.Database.IsSqlServer())
    {
        dbContext.Database.Migrate(); // veritaban� migrasyonlar�n� uygula
    }

    SeedData.Initialize(dbContext); // veritaban�n� seed et
}


// 6) Pipeline
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
