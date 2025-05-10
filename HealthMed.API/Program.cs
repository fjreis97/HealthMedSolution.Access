using Health_Med.Business;
using Health_Med.Business.Interfaces;
using Health_Med.Domain.Mappings;
using Health_Med.Infrastructure.DAL;
using Health_Med.Infrastructure.Repositories.Appointment;
using Health_Med.Infrastructure.Repositories.Collaborator;
using Health_Med.Infrastructure.Repositories.Doctor;
using Health_Med.Infrastructure.Repositories.DoctorByEspecialty;
using Health_Med.Infrastructure.Repositories.DoctorByService;
using Health_Med.Infrastructure.Repositories.Interface;
using Health_Med.Infrastructure.Repositories.MedialEspecialty;
using Health_Med.Infrastructure.Repositories.MedicalSchedule;
using Health_Med.Infrastructure.Repositories.MedicalService;
using Health_Med.Infrastructure.Repositories.Patient;
using Health_Med.Infrastructure.Repositories.ServiceHoursDoctor;
using Health_Med.Infrastructure.UnitOfWork;
using Health_Med.Infrastructure.UnitOfWork.Interface;
using HealthMed.API.Access.Common.ColetorErrors;
using HealthMed.API.Access.Common.ColetorErrors.Interfaces;
using HealthMed.API.Access.Common.HashGenerator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!; 

//register the unit of work in the DI container
builder.Services.AddScoped<BdHealthMedSession>(provider => new BdHealthMedSession(connectionString));

//Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//add service to the coletor errors
builder.Services.AddScoped<IColetorErrors, ColetorErrors>();
builder.Services.AddScoped<IPasswordGenerate, PasswordGenerate>();

//add services to the Repositories
builder.Services.AddScoped<ICollaboratorRepository, CollaboratorRepository>();
builder.Services.AddScoped<IMedicalServiceRepository, MedicalServiceRepository>();
builder.Services.AddScoped<IMedicalEspecialtyRepository, MedicalEspecialtyRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IDoctorByServiceRepository, DoctorByServiceRepository>();
builder.Services.AddScoped<IDoctorByEspecialtyRepository, DoctorByEspecialtyRepository>();
builder.Services.AddScoped<IServiceHoursRepository, ServiceHoursRepository>();
builder.Services.AddScoped<IMedicalScheduleRepository, MedicalScheduleRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

//add service to the unit of work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add services to the container.
builder.Services.AddScoped<ICollaboratorService, CollaboratorService>();
builder.Services.AddScoped<IMedicalServiceService, MedicalServiceService>();
builder.Services.AddScoped<IMedicalEspecialtyService, MedicalEspecialtyService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IDoctorByServiceService, DoctorByServiceService>();
builder.Services.AddScoped<IDoctorByEspecialtyService, DoctorByEspecialtyService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IServiceHoursService, ServiceHoursService>();
builder.Services.AddScoped<IMedicalScheduleService, MedicalScheduleService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();

var key = Encoding.ASCII.GetBytes(builder.Configuration["SecretJWT"]!);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false

    };

});



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API de Cadastros - HealthMed ", Version = "v1" });

    //para documentar o swagger
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    // Definir o esquema de segurança
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor, insira o token JWT com o prefixo Bearer no campo de texto abaixo.\nExemplo: \"Bearer {token}\"",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
// Remova o if
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
