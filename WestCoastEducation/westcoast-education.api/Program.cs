using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using westcoast_education.api.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Database connection
builder.Services.AddDbContext<WestcoastEducationsContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Westcoast Education API",
        Description = "Ett api för att hantera kurser, lärare samt studenter",
        TermsOfService = new Uri("https://westcoast-education.se/terms"),
        Contact = new OpenApiContact
        {
            Name = "Teknisk Support",
            Url = new Uri("https://westcoast-education.se/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Licens",
            Url = new Uri("https://westcoast-education.se/license")
        }
    });
    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
});

builder.Services.AddCors();

var app = builder.Build();

// Seed the database...
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<WestcoastEducationsContext>();
    await context.Database.MigrateAsync();

    await SeedData.LoadTeacherData(context);
    await SeedData.LoadStudentData(context);
    await SeedData.LoadCourseData(context);
    await SeedData.LoadCompetenceData(context);
}
catch (Exception ex)
{
    Console.WriteLine("{0}", ex.Message);
    throw;
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(c => c.AllowAnyHeader()
    .AllowAnyMethod()
    .WithOrigins("http://127.0.0.1:5500"));

app.MapControllers();

app.Run();
