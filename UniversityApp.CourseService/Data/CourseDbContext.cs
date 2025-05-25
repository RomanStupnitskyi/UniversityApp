using Microsoft.EntityFrameworkCore;
using UniversityApp.Shared.Models;

namespace UniversityApp.CourseService.Data;

public class CourseDbContext(DbContextOptions<CourseDbContext> options) : DbContext(options)
{
	public DbSet<Course> Courses { get; set; }
	
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Course>()
			.HasKey(c => c.Id);

		modelBuilder.Entity<Course>()
			.Property(c => c.Title)
			.IsRequired()
			.HasMaxLength(100);

		modelBuilder.Entity<Course>()
			.Property(c => c.Description)
			.IsRequired()
			.HasMaxLength(500);

		modelBuilder.Entity<Course>()
			.Property(c => c.ECTS)
			.IsRequired();
	}
}