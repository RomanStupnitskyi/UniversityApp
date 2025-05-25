using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using UniversityApp.UserService.Data;
using UniversityApp.Shared.Models;

namespace UniversityApp.UserService.Repositories;

public class StudentRepository(UserDbContext dbContext) : IStudentRepository
{
	[SuppressMessage("ReSharper.DPA", "DPA0000: DPA issues")]
	public async Task<IEnumerable<Student>> GetAllAsync()
	{
		return await dbContext.Students.ToListAsync();
	}

	public async Task<Student?> GetByIdAsync(Guid id)
	{
		return await dbContext.Students.FindAsync(id);
	}

	public async Task AddAsync(Student student)
	{
		await dbContext.Students.AddAsync(student);
		await dbContext.SaveChangesAsync();
	}

	public async Task UpdateAsync(Student student)
	{
		dbContext.Students.Update(student);
		await dbContext.SaveChangesAsync();
	}

	public async Task DeleteAsync(Student student)
	{
		dbContext.Students.Remove(student);
		await dbContext.SaveChangesAsync();
	}

	public async Task<bool> DeleteByIdAsync(Guid id)
	{
		var student = await GetByIdAsync(id);
		if (student == null)
			return false;
		
		await DeleteAsync(student);
		return true;
	}
	
	public async Task<Student?> FindStudentByStudentNumber(string studentNumber)
	{
		return await dbContext.Students
			.FirstOrDefaultAsync(s => s.StudentNumber == studentNumber);
	}
}