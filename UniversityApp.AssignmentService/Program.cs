using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Refit;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using UniversityApp.AssignmentService.API;
using UniversityApp.AssignmentService.Consumers.Assignments;
using UniversityApp.AssignmentService.Consumers.Courses;
using UniversityApp.AssignmentService.Data;
using UniversityApp.AssignmentService.Extensions;
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
// -- MassTransit & RabbitMQ
// -------------------------------------------------------------------------------
builder.Services.AddMassTransit(configurator =>
{
	configurator.SetKebabCaseEndpointNameFormatter();

	configurator.AddConsumer<AssignmentDeletedConsumer>();
	configurator.AddConsumer<AssignmentsDeletedConsumer>();
	configurator.AddConsumer<CourseDeletedConsumer>();
	
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
// app.UsePathBase("/api/assignments"); // Set the base path for the application

// -------------------------------------------------------------------------------
// -- Middlewares
// -------------------------------------------------------------------------------
app.ApplyMigrations(); // Apply database migrations at startup
app.UseOpenApi(); // Serves the registered OpenAPI/Swagger documents
app.UseSwaggerUi(); // Serves the Swagger UI

app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS
app.UseAuthorization(); // Enables authorization middleware
app.MapControllers(); // Maps attribute-routed controllers to the app

// -------------------------------------------------------------------------------
// -- Run the application
// -------------------------------------------------------------------------------
app.Run();