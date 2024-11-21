using Pomodoro.API.DATA;
using Microsoft.EntityFrameworkCore;
using Pomodoro.API.Helpers;
using Pomodoro.Shared.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddControllers();

// Configurar Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>

{

    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pomodoro API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme

    {

        Description = @"JWT Authorization header using the Bearer scheme. <br /> <br /> 

                      Enter 'Bearer' [space] and then your token in the text input below.<br /> <br /> 

                      Example: 'Bearer 12345abcdef'<br /> <br />",

        Name = "Authorization",

        In = ParameterLocation.Header,

        Type = SecuritySchemeType.ApiKey,

        Scheme = "Bearer"

    });

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

              Scheme = " Bearer ",

              Name = "Bearer",

              In = ParameterLocation.Header,

            },

            new List<string>()

          }

        });

});

// Agregar el servicio de DbContext con la cadena de conexión definida en appsettings.json
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Agregar el servicio de SeedDb para inicializar la base de datos
builder.Services.AddTransient<SeedDb>();



builder.Services.AddIdentity<User, IdentityRole>(x =>

{

    x.User.RequireUniqueEmail = true;

    x.Password.RequireDigit = false;

    x.Password.RequiredUniqueChars = 0;

    x.Password.RequireLowercase = false;

    x.Password.RequireNonAlphanumeric = false;

    x.Password.RequireUppercase = false;

})

    .AddEntityFrameworkStores<DataContext>()

    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(x => x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true, 
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwtKey"]!)),
        ClockSkew = TimeSpan.Zero,
    });




builder.Services.AddScoped<IUserHelper, UserHelper>();

var app = builder.Build();

// Alimentar la base de datos con SeedData
SeedData(app);

void SeedData(WebApplication app)
{
    IServiceScopeFactory? scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (IServiceScope? scope = scopedFactory!.CreateScope())
    {
        SeedDb? service = scope.ServiceProvider.GetService<SeedDb>();
        service!.SeedAsync().Wait();
    }
}

// Configurar el pipeline de solicitudes HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Habilitar CORS para que la API pueda ser consumida desde otros orígenes
app.UseCors(policy => policy
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

app.MapControllers();

app.Run();
