using UniversityApp.Shared.Models;

namespace UniversityApp.CourseService.Repositories;

public interface ICourseRepository
{
	Task<IEnumerable<Course>> GetAllAsync();
	Task<Course?> GetByIdAsync(string id);
	Task<bool> AddAsync(Course course);
	Task<bool> UpdateAsync(Course course);
	Task<bool> DeleteAsync(Course course);
	Task<bool> DeleteByIdAsync(string id);
}