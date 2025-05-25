using Microsoft.EntityFrameworkCore;
using UniversityApp.AssignmentService.Data;
using UniversityApp.Shared.Models;

namespace UniversityApp.AssignmentService.Repositories;

public class AssignmentRepository(AssignmentDbContext assignmentDbContext) : IAssignmentRepository
{
	public async Task<IEnumerable<Assignment>> GetAllAsync()
	{
		var assignments = await assignmentDbContext.Assignments.ToListAsync();
		return assignments;
	}

	public async Task<Assignment?> GetByIdAsync(Guid id)
	{
		var assignment = await assignmentDbContext.Assignments
			.FirstOrDefaultAsync(a => a.Id == id);
		return assignment;
	}

	public async Task AddAsync(Assignment assignment)
	{
		await assignmentDbContext.Assignments.AddAsync(assignment);
		await assignmentDbContext.SaveChangesAsync();
	}

	public async Task UpdateAsync(Assignment assignment)
	{
		assignmentDbContext.Assignments.Update(assignment);
		await assignmentDbContext.SaveChangesAsync();
	}

	public async Task DeleteAsync(Assignment assignment)
	{
		assignmentDbContext.Assignments.Remove(assignment);
		await assignmentDbContext.SaveChangesAsync();
	}

	public async Task<bool> DeleteByCourseIdAsync(Guid id)
	{
		var assignments = await assignmentDbContext.Assignments
			.Where(a => a.CourseId == id)
			.ToListAsync();

		if (assignments.Count == 0)
			return true;

		assignmentDbContext.Assignments.RemoveRange(assignments);
		await assignmentDbContext.SaveChangesAsync();
		return true;
	}

	public async Task<bool> DeleteByIdAsync(Guid id)
	{
		var assignment = await GetByIdAsync(id);
		if (assignment == null)
			return false;

		await DeleteAsync(assignment);
		return true;
	}
}