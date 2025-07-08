using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NZWalksAPI.AutoMappings;
using NZWalksAPI.Data;
using NZWalksAPI.Repositories;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();


//to serve static files like images
builder.Services.AddHttpContextAccessor();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//adding authentication in swagger
builder.Services.AddSwaggerGen(options=>
    {
     options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo {Title = "NZ Walks API",Version = "v1", Description = "API for managing NZ Walks"});
     options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new Microsoft.OpenApi.Models.OpenApiSecurityScheme
     {
         Name = "Authorization",
         In = Microsoft.OpenApi.Models.ParameterLocation.Header,
         Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
         Scheme = JwtBearerDefaults.AuthenticationScheme,
     });
     options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme,
                },
                Scheme = "Oath2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = Microsoft.OpenApi.Models.ParameterLocation.Header
            },
            new List<string>()
        }
    });
    });

//1- Register the  App DbContext with dependency injection
var connectionString = builder.Configuration.GetConnectionString("NZWalksConnectionString");
builder.Services.AddDbContext<NZWalksDbContext>(options =>
    options.UseSqlServer(connectionString));

//5- Register the authentication database context with dependency injection
var authConnectionString = builder.Configuration.GetConnectionString("NZWalksAuthConnectionString");
builder.Services.AddDbContext<NZWalksAuthDbContext>(options =>
    options.UseSqlServer(authConnectionString));

//2-  Register the repository with dependency injection
builder.Services.AddScoped<IRegionRepository, SQLRegionRepository>();
builder.Services.AddScoped<IWalkRepository, SQLWalkRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IImageRepository, LocalImageRepository>();

//3-  Register AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

// Register Identity services
builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>() // Add roles support
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NZWalksAuth")
    .AddEntityFrameworkStores<NZWalksAuthDbContext>()
    .AddDefaultTokenProviders();

//6- Configure Identity options
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

//5- Configure authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidIssuer = builder.Configuration["Jwt:Issuer"],
         ValidAudience = builder.Configuration["Jwt:Audience"],
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
     }
    );

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

app.MapControllers();

app.Run();
