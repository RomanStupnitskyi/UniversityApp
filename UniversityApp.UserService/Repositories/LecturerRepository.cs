using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UniversityApp.UserService.Data;
using UniversityApp.Shared.Models;
using UniversityApp.UserService.Specifications;

namespace UniversityApp.UserService.Repositories;

public class LecturerRepository(UserDbContext dbContext) : ILecturerRepository
{
	public async Task<IEnumerable<Lecturer>> GetAllAsync(LecturerSpecification specification)
	{
		return await dbContext
			.Lecturers
			.WithSpecification(specification)
			.ToListAsync();
	}

	public async Task<Lecturer?> GetByIdAsync(Guid id)
	{
		return await dbContext.Lecturers.FindAsync(id);
	}

	public async Task AddAsync(Lecturer lecturer)
	{
		await dbContext.Lecturers.AddAsync(lecturer);
		await dbContext.SaveChangesAsync();
	}

	public async Task UpdateAsync(Lecturer lecturer)
	{
		dbContext.Lecturers.Update(lecturer);
		await dbContext.SaveChangesAsync();
	}

	public async Task DeleteAsync(Lecturer lecturer)
	{
		dbContext.Lecturers.Remove(lecturer);
		await dbContext.SaveChangesAsync();
	}

	public async Task<bool> DeleteByIdAsync(Guid id)
	{
		var lecturer = await GetByIdAsync(id);
		if (lecturer == null)
			return false;
		
		await DeleteAsync(lecturer);
		return true;
	}
}