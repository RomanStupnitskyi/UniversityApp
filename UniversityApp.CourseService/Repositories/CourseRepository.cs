using Microsoft.EntityFrameworkCore;
using UniversityApp.CourseService.Data;
using UniversityApp.Shared.Models;

namespace UniversityApp.CourseService.Repositories;

public class CourseRepository(CourseDbContext dbContext) : ICourseRepository
{
	public async Task<IEnumerable<Course>> GetAllAsync()
	{
		var courses = await dbContext.Courses.ToListAsync();
		return courses;
	}

	public Task<Course?> GetByIdAsync(string id)
	{
		var course = dbContext.Courses.FirstOrDefaultAsync(c => c.Id == id);
		return course;
	}

	public async Task<bool> AddAsync(Course course)
	{
		try
		{
			await dbContext.Courses.AddAsync(course);
			await dbContext.SaveChangesAsync();
			
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public async Task<bool> UpdateAsync(Course course)
	{
		try
		{
			dbContext.Courses.Update(course);
			await dbContext.SaveChangesAsync();
			
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public async Task<bool> DeleteAsync(Course course)
	{
		try
		{
			dbContext.Courses.Remove(course);
			await dbContext.SaveChangesAsync();

			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public async Task<bool> DeleteByIdAsync(string id)
	{
		var course = await GetByIdAsync(id);
		if (course == null)
			throw new KeyNotFoundException($"Student with ID {id} not found.");
		
		try
		{
			return await DeleteAsync(course);
		}
		catch (Exception)
		{
			return false;
		}
	}
}