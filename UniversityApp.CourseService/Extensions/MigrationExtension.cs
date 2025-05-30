using Microsoft.EntityFrameworkCore;
using UniversityApp.CourseService.Data;

namespace UniversityApp.CourseService.Extensions;

public static class MigrationExtension
{
	public static void ApplyMigrations(this IApplicationBuilder app)
	{
		using var scope = app.ApplicationServices.CreateScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<CourseDbContext>();
		dbContext.Database.Migrate();
	}
}