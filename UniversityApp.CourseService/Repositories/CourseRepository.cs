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

	public Task<Course?> GetByIdAsync(Guid id)
	{
		var course = dbContext.Courses.FirstOrDefaultAsync(c => c.Id == id);
		return course;
	}

	public async Task AddAsync(Course course)
	{
		await dbContext.Courses.AddAsync(course);
		await dbContext.SaveChangesAsync();
	}

	public async Task UpdateAsync(Course course)
	{
		dbContext.Courses.Update(course);
		await dbContext.SaveChangesAsync();
	}

	public async Task DeleteAsync(Course course)
	{
		dbContext.Courses.Remove(course);
		await dbContext.SaveChangesAsync();
	}

	public async Task<bool> DeleteByIdAsync(Guid id)
	{
		var course = await GetByIdAsync(id);
		if (course == null)
			return false;
		
		await DeleteAsync(course);
		return true;
	}
}