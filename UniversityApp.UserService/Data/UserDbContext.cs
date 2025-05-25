using Microsoft.EntityFrameworkCore;
using UniversityApp.Shared.Models;

namespace UniversityApp.UserService.Data;

public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
{
	public DbSet<Student> Students => Set<Student>();
	public DbSet<Lecturer> Lecturers => Set<Lecturer>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Student>()
			.ToTable("Students")
			.HasIndex(x => x.StudentNumber)
			.IsUnique();

		modelBuilder.Entity<Lecturer>()
			.ToTable("Lecturers");
	}
}