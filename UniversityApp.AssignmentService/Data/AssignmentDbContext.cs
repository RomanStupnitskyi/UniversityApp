using Microsoft.EntityFrameworkCore;
using UniversityApp.Shared.Models;

namespace UniversityApp.AssignmentService.Data;

public class AssignmentDbContext(DbContextOptions<AssignmentDbContext> options) : DbContext(options)
{
	public DbSet<Assignment> Assignments { get; set; }
	public DbSet<Submission> Submissions { get; set; }
	
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// ----------------------------------------------------------
		// -- Assignments
		// ----------------------------------------------------------
		modelBuilder.Entity<Assignment>()
			.ToTable("Assignments");
		
		modelBuilder.Entity<Assignment>()
			.HasKey(a => a.Id);

		modelBuilder.Entity<Assignment>()
			.Property(a => a.Title)
			.IsRequired()
			.HasMaxLength(100);

		modelBuilder.Entity<Assignment>()
			.Property(a => a.Description)
			.IsRequired()
			.HasMaxLength(500);
		
		// ----------------------------------------------------------
		// -- Submissions
		// ----------------------------------------------------------
		modelBuilder.Entity<Submission>()
			.ToTable("Submissions");
		
		modelBuilder.Entity<Submission>()
			.HasKey(a => a.Id);
		
		modelBuilder.Entity<Submission>()
			.Property(a => a.Content)
			.IsRequired()
			.HasMaxLength(500);
	}
}