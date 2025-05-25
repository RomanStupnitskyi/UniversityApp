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

	public async Task<Student?> GetByIdAsync(string id)
	{
		return await dbContext.Students.FindAsync(id);
	}

	public async Task<bool> AddAsync(Student student)
	{
		try
		{
			await dbContext.Students.AddAsync(student);
			await dbContext.SaveChangesAsync();
			
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public async Task<bool> UpdateAsync(Student student)
	{
		try
		{
			dbContext.Students.Update(student);
			await dbContext.SaveChangesAsync();
			
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public async Task DeleteAsync(Student student)
	{
		dbContext.Students.Remove(student);
		await dbContext.SaveChangesAsync();
	}

	public async Task<bool> DeleteByIdAsync(string id)
	{
		var student = await GetByIdAsync(id);
		if (student == null)
			throw new KeyNotFoundException($"Student with ID {id} not found.");
		
		try
		{
			await DeleteAsync(student);
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}
	
	public async Task<Student?> FindStudentByStudentNumber(string studentNumber)
	{
		return await dbContext.Students
			.FirstOrDefaultAsync(s => s.StudentNumber == studentNumber);
	}
}