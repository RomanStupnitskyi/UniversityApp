using Microsoft.OpenApi.Models;

namespace UniversityApp.CourseService.Extensions;

internal static class ServiceCollectionExtensions
{
	internal static void AddSwaggerGenWithAuthentication(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddSwaggerGen(options =>
		{
			options.CustomSchemaIds(id => id.FullName!.Replace('+', '-'));
			
			options.AddSecurityDefinition("Keycloak", new OpenApiSecurityScheme
			{
				Type = SecuritySchemeType.OAuth2,
				Flows = new OpenApiOAuthFlows
				{
					AuthorizationCode = new OpenApiOAuthFlow
					{
						AuthorizationUrl = new Uri(configuration["Keycloak:AuthorizationUrl"]
						                           ?? throw new InvalidOperationException("Keycloak:AuthorizationUrl is not configured")),
						TokenUrl = new Uri(configuration["Keycloak:TokenUrl"]
						                   ?? throw new InvalidOperationException("Keycloak:TokenUrl is not configured")),
						Scopes = new Dictionary<string, string>
						{
							{ "openid", "OpenID Connect scope" },
							{ "profile", "User profile" },
							{ "email", "User email" }
						}
					}
				}
			});

			var securityRequirement = new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Id = "Keycloak",
							Type = ReferenceType.SecurityScheme
						},
						In = ParameterLocation.Header,
						Name = "Bearer",
						Scheme = "Bearer"
					},
					[]
				}
			};
			
			options.AddSecurityRequirement(securityRequirement);
		});
	}
}