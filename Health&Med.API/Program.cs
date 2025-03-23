using Health_Med.Business;
using Health_Med.Business.Interfaces;
using Health_Med.Domain.Mappings;
using Health_Med.Infrastructure.DAL;
using Health_Med.Infrastructure.Repositories.Collaborator;
using Health_Med.Infrastructure.Repositories.Doctor;
using Health_Med.Infrastructure.Repositories.DoctorByEspecialty;
using Health_Med.Infrastructure.Repositories.DoctorByService;
using Health_Med.Infrastructure.Repositories.Interface;
using Health_Med.Infrastructure.Repositories.MedialEspecialty;
using Health_Med.Infrastructure.Repositories.MedicalService;
using Health_Med.Infrastructure.Repositories.Patient;
using Health_Med.Infrastructure.UnitOfWork;
using Health_Med.Infrastructure.UnitOfWork.Interface;
using HealthMed.API.Access.Common.ColetorErrors;
using HealthMed.API.Access.Common.ColetorErrors.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!; 

//register the unit of work in the DI container
builder.Services.AddScoped<BdHealthMedSession>(provider => new BdHealthMedSession(connectionString));

//Automapper
builder.Services.AddAutoMapper(typeof(CollaboratorProfile));
builder.Services.AddAutoMapper(typeof(MedicalServiceProfile));
builder.Services.AddAutoMapper(typeof(MedicalEspecialtyProfile));
builder.Services.AddAutoMapper(typeof(DoctorProfile));
builder.Services.AddAutoMapper(typeof(PatientProfile));
builder.Services.AddAutoMapper(typeof(DoctorByServiceProfile));
builder.Services.AddAutoMapper(typeof(DoctorByEspecialtyProfile));

//add service to the coletor errors
builder.Services.AddScoped<IColetorErrors, ColetorErrors>();

//add services to the Repositories
builder.Services.AddScoped<ICollaboratorRepository, CollaboratorRepository>();
builder.Services.AddScoped<IMedicalServiceRepository, MedicalServiceRepository>();
builder.Services.AddScoped<IMedicalEspecialtyRepository, MedicalEspecialtyRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IDoctorByServiceRepository, DoctorByServiceRepository>();
builder.Services.AddScoped<IDoctorByEspecialtyRepository, DoctorByEspecialtyRepository>();

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


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
