using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Refit;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using UniversityApp.AssignmentService.API;
using UniversityApp.AssignmentService.Data;
using UniversityApp.AssignmentService.Repositories;
using UniversityApp.AssignmentService.Services;
using UniversityApp.AssignmentService.Validators;

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
builder.Services.AddDbContext<AssignmentDbContext>(options =>
{
	options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// -------------------------------------------------------------------------------
// -- HttpClient
// -------------------------------------------------------------------------------
builder.Services.AddRefitClient<ICourseAPI>()
	.ConfigureHttpClient(options =>
	{
		options.BaseAddress = new Uri(builder.Configuration["API:CourseAPI"]
		    ?? throw new Exception("Course API URL is not configured"));
	});
builder.Services.AddRefitClient<IUserAPI>()
	.ConfigureHttpClient(options =>
	{
		options.BaseAddress = new Uri(builder.Configuration["API:UserAPI"]
		                              ?? throw new Exception("User API URL is not configured"));
	});

// -------------------------------------------------------------------------------
// -- Dependency Injection
// -------------------------------------------------------------------------------
builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();
builder.Services.AddScoped<IAssignmentService, AssignmentService>();
builder.Services.AddScoped<ISubmissionRepository, SubmissionRepository>();
builder.Services.AddScoped<ISubmissionService, SubmissionService>();

// -------------------------------------------------------------------------------
// -- FluentValidation
// -------------------------------------------------------------------------------
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateAssignmentValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateSubmissionValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateAssignmentValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateSubmissionValidator>();

// -------------------------------------------------------------------------------
// -- Controllers
// -------------------------------------------------------------------------------
builder.Services.AddControllers();

var app = builder.Build(); // Build the application pipeline
app.UsePathBase("/api/assignments"); // Set the base path for the application

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