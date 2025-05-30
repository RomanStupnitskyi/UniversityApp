using Microsoft.EntityFrameworkCore;
using UniversityApp.AssignmentService.Data;

namespace UniversityApp.AssignmentService.Extensions;

public static class MigrationExtension
{
	public static void ApplyMigrations(this IApplicationBuilder app)
	{
		using var scope = app.ApplicationServices.CreateScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<AssignmentDbContext>();
		dbContext.Database.Migrate();
	}
}