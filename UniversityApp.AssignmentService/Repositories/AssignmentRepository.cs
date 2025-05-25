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

	public async Task<Assignment?> GetByIdAsync(string id)
	{
		var assignment = await assignmentDbContext.Assignments
			.FirstOrDefaultAsync(a => a.Id == id);
		return assignment;
	}

	public async Task<bool> AddAsync(Assignment assignment)
	{
		try
		{
			await assignmentDbContext.Assignments.AddAsync(assignment);
			await assignmentDbContext.SaveChangesAsync();
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public async Task<bool> UpdateAsync(Assignment assignment)
	{
		try
		{
			assignmentDbContext.Assignments.Update(assignment);
			await assignmentDbContext.SaveChangesAsync();
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public async Task<bool> DeleteAsync(Assignment assignment)
	{
		try
		{
			assignmentDbContext.Assignments.Remove(assignment);
			await assignmentDbContext.SaveChangesAsync();
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public async Task<bool> DeleteByIdAsync(string id)
	{
		var assignment = await GetByIdAsync(id);
		if (assignment == null)
			throw new KeyNotFoundException($"Assignment with ID=\"{id}\" not found.");

		try
		{
			return await DeleteAsync(assignment);
		}
		catch (Exception)
		{
			return false;
		}
	}
}