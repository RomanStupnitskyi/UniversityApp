using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------------------------------
// -- Add services to the container.
// ----------------------------------------------------------------
builder.Services.AddEndpointsApiExplorer();

// -------------------------------------------------------------------------------
// -- OpenTelemetry
// -------------------------------------------------------------------------------
builder.Services
	.AddOpenTelemetry()
	.ConfigureResource(resource => resource.AddService("UniversityApp.ApiGateway"))
	.WithTracing(options =>
	{
		options
			.AddAspNetCoreInstrumentation()
			.AddHttpClientInstrumentation();

		options.AddOtlpExporter(configure =>
		{
			configure.Endpoint = new Uri(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]
			                             ?? throw new Exception("OTLP Exporter Endpoint is not configured"));
		});
	});

// ----------------------------------------------------------------
// -- Add Ocelot configuration from JSON file.
// ----------------------------------------------------------------
builder.Configuration.AddJsonFile("ocelot.json", false, true);
builder.Services.AddOcelot();

// ----------------------------------------------------------------
// -- Add CORS policy to allow requests from specific origins.
// ----------------------------------------------------------------
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowFrontendOrigins",
		policyBuilder => policyBuilder.WithOrigins("https://localhost:3001")
			.AllowAnyMethod()
			.AllowAnyHeader()
			.AllowCredentials());
});

// ----------------------------------------------------------------
// -- Add Swagger for Ocelot.
// ----------------------------------------------------------------
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerForOcelot(builder.Configuration);


var app = builder.Build();

// ----------------------------------------------------------------
// -- Add middleware to the request pipeline.
// ----------------------------------------------------------------
app.UseCors("AllowFrontendOrigins"); // Use the CORS policy defined above

app.UseHttpsRedirection(); // Enable HTTPS redirection

// Configure Swagger for Ocelot
app.UseSwaggerForOcelotUI(opt =>
{
	opt.PathToSwaggerGenerator = "/swagger/docs";
});

await app.UseOcelot(); // Use Ocelot middleware for routing

app.Run();