using Microsoft.EntityFrameworkCore;
using UniversityApp.UserService.Data;

namespace UniversityApp.UserService.Extensions;

public static class MigrationExtension
{
	public static void ApplyMigrations(this IApplicationBuilder app)
	{
		using var scope = app.ApplicationServices.CreateScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
		dbContext.Database.Migrate();
	}
}