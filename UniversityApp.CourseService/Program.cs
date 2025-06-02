using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using UniversityApp.CourseService.Data;
using UniversityApp.CourseService.Extensions;
using UniversityApp.CourseService.Repositories;
using UniversityApp.CourseService.Services;
using UniversityApp.CourseService.Validators;

var builder = WebApplication.CreateBuilder(args);

// -------------------------------------------------------------------------------
// -- Swagger
// -------------------------------------------------------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenWithAuthentication(builder.Configuration);

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
builder.Services.AddDbContext<CourseDbContext>(options =>
{
	options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// -------------------------------------------------------------------------------
// -- Authentication
// -------------------------------------------------------------------------------
builder.Services.AddAuthorizationBuilder()
	.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("admin"))
	.AddPolicy("RequireLecturerRole", policy => policy.RequireRole("lecturer"))
	.AddPolicy("RequireStudentRole", policy => policy.RequireRole("student"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.Authority = builder.Configuration["Authentication:DockerRealmUrl"]
		                    ?? throw new Exception("Docker Realm URL for JWT Bearer is not configured");
		options.Audience = builder.Configuration["Authentication:Audience"]
		                   ?? throw new Exception("Audience for JWT Bearer is not configured");
		options.MetadataAddress = builder.Configuration["Authentication:MetadataAddress"]
		                          ?? throw new Exception("Metadata address for JWT Bearer is not configured");

		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateAudience = false
		};
		
		options.RequireHttpsMetadata = false;

		options.TokenValidationParameters.ValidIssuers =
		[
			builder.Configuration["Authentication:RealmUrl"]
			?? throw new Exception("Realm URL for JWT Bearer is not configured"),
			builder.Configuration["Authentication:AppRealmUrl"]
			?? throw new Exception("App Realm URL for JWT Bearer is not configured")
		];
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
// app.UsePathBase("/api/courses"); // Set the base path for the application

// -------------------------------------------------------------------------------
// -- Middlewares
// -------------------------------------------------------------------------------
app.ApplyMigrations(); // Apply database migrations at startup
app.UseSwagger(); // Enable Swagger for API documentation
app.UseSwaggerUI(options =>
{
	options.SwaggerEndpoint("/swagger/v1/swagger.json", "UniversityApp User Service API V1");
	
	options.OAuthClientId("university-frontend-app");
	options.OAuthUsePkce();
	options.OAuthScopes("openid", "profile", "email");
});

app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS
app.UseAuthorization(); // Enables authorization middleware
app.MapControllers(); // Maps attribute-routed controllers to the app

// -------------------------------------------------------------------------------
// -- Run the application
// -------------------------------------------------------------------------------
app.Run();