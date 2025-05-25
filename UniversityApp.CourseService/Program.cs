using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using UniversityApp.CourseService.Data;
using UniversityApp.CourseService.Repositories;
using UniversityApp.CourseService.Services;
using UniversityApp.CourseService.Validators;

var builder = WebApplication.CreateBuilder(args);

// -------------------------------------------------------------------------------
// -- Swagger
// -------------------------------------------------------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(options =>
{
	options.DocumentName = "v1";
	options.Title = "UniversityApp Courses Service API";
});

// -------------------------------------------------------------------------------
// -- Database
// -------------------------------------------------------------------------------
builder.Services.AddDbContext<CourseDbContext>(options =>
{
	options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// -------------------------------------------------------------------------------
// -- Dependency Injection
// -------------------------------------------------------------------------------
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseService, CourseService>();

// -------------------------------------------------------------------------------
// -- FluentValidation
// -------------------------------------------------------------------------------
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateCourseValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateCourseValidator>();

// -------------------------------------------------------------------------------
// -- Controllers
// -------------------------------------------------------------------------------
builder.Services.AddControllers();

var app = builder.Build(); // Build the application pipeline
app.UsePathBase("/api/courses"); // Set the base path for the application

// -------------------------------------------------------------------------------
// -- Middlewares
// -------------------------------------------------------------------------------
app.UseOpenApi(); // Serves the registered OpenAPI/Swagger documents
app.UseSwaggerUi(); // Serves the Swagger UI

app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS
app.UseAuthorization(); // Enables authorization middleware
app.MapControllers(); // Maps attribute-routed controllers to the app

// -------------------------------------------------------------------------------
// -- Run the application
// -------------------------------------------------------------------------------
app.Run();