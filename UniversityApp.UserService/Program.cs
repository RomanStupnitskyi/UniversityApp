using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using UniversityApp.UserService.Data;
using UniversityApp.UserService.Extensions;
using UniversityApp.UserService.Repositories;
using UniversityApp.UserService.Services;
using UniversityApp.UserService.Validators;

var builder = WebApplication.CreateBuilder(args);

// -------------------------------------------------------------------------------
// -- CORS
// -------------------------------------------------------------------------------
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAllOrigins",
		policyBuilder => policyBuilder.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader());
});

// -------------------------------------------------------------------------------
// -- Swagger
// -------------------------------------------------------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(options =>
{
	options.DocumentName = "v1";
	options.Title = "UniversityApp User Service API";
});

// -------------------------------------------------------------------------------
// -- Exceptions handler
// -------------------------------------------------------------------------------
builder.Services.AddProblemDetails(options =>
{
	options.CustomizeProblemDetails = context =>
	{
		context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
		context.ProblemDetails.Extensions.Add("requestId", context.HttpContext.TraceIdentifier);

		var activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
		context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
	};
});
// builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// -------------------------------------------------------------------------------
// -- Database
// -------------------------------------------------------------------------------
builder.Services.AddDbContext<UserDbContext>(options =>
{
	options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// -------------------------------------------------------------------------------
// -- MassTransit & RabbitMQ
// -------------------------------------------------------------------------------
builder.Services.AddMassTransit(configurator =>
{
	configurator.SetKebabCaseEndpointNameFormatter();
	
	configurator.UsingRabbitMq((context, factoryConfigurator) =>
	{
		factoryConfigurator.Host(builder.Configuration["MessageBroker:Hostname"] ?? throw new Exception("RabbitMQ Hostname is not configured"),
			ushort.Parse(builder.Configuration["MessageBroker:Port"] ?? "5672"),
			"/",
			hostConfigurator =>
		{
			hostConfigurator.Username(builder.Configuration["MessageBroker:Username"]
			                          ?? throw new Exception("RabbitMQ Username is not configured"));
			hostConfigurator.Password(builder.Configuration["MessageBroker:Password"]
			                          ?? throw new Exception("RabbitMQ Password is not configured"));
		});
			
		factoryConfigurator.ConfigureEndpoints(context);
	});
});

// -------------------------------------------------------------------------------
// -- Dependency Injection
// -------------------------------------------------------------------------------
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();

builder.Services.AddScoped<ILecturerRepository, LecturerRepository>();
builder.Services.AddScoped<ILecturerService, LecturerService>();

// -------------------------------------------------------------------------------
// -- FluentValidation
// -------------------------------------------------------------------------------
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateStudentValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateLecturerValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateStudentValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateLecturerValidator>();

// -------------------------------------------------------------------------------
// -- Controllers
// -------------------------------------------------------------------------------
builder.Services.AddControllers();

var app = builder.Build(); // Build the application pipeline
// app.UsePathBase("/api/users"); // Set the base path for the API

// -------------------------------------------------------------------------------
// -- Middlewares
// -------------------------------------------------------------------------------
app.UseCors("AllowAllOrigins"); // Apply CORS policy to allow all origins, methods, and headers
app.ApplyMigrations(); // Apply database migrations at startup
app.UseOpenApi(); // Serves the registered OpenAPI/Swagger documents
app.UseSwaggerUi(options =>
{
	// Set prefix /api/users for the Swagger UI
	options.DocumentPath = "/api/users/swagger/v1/swagger.json";
}); // Serves the Swagger UI

app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS
app.UseAuthorization(); // Enables authorization middleware
app.MapControllers(); // Maps attribute-routed controllers to the app

// -------------------------------------------------------------------------------
// -- Run the application
// -------------------------------------------------------------------------------
app.Run();