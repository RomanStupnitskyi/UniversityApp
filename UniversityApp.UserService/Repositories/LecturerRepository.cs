using Microsoft.EntityFrameworkCore;
using UniversityApp.UserService.Data;
using UniversityApp.Shared.Models;

namespace UniversityApp.UserService.Repositories;

public class LecturerRepository(UserDbContext dbContext) : ILecturerRepository
{
	public async Task<IEnumerable<Lecturer>> GetAllAsync()
	{
		return await dbContext.Lecturers.ToListAsync();
	}

	public async Task<Lecturer?> GetByIdAsync(string id)
	{
		return await dbContext.Lecturers.FindAsync(id);
	}

	public async Task<bool> AddAsync(Lecturer lecturer)
	{
		try
		{
			await dbContext.Lecturers.AddAsync(lecturer);
			await dbContext.SaveChangesAsync();
			
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public async Task<bool> UpdateAsync(Lecturer lecturer)
	{
		try
		{
			dbContext.Lecturers.Update(lecturer);
			await dbContext.SaveChangesAsync();
			
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public async Task DeleteAsync(Lecturer lecturer)
	{
		dbContext.Lecturers.Remove(lecturer);
		await dbContext.SaveChangesAsync();
	}

	public async Task<bool> DeleteByIdAsync(string id)
	{
		var lecturer = await GetByIdAsync(id);
		if (lecturer == null)
			throw new KeyNotFoundException($"Student with ID {id} not found.");
		
		try
		{
			await DeleteAsync(lecturer);
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}
}