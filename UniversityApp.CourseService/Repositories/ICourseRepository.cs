using Ardalis.Specification;
using UniversityApp.Shared.Models;

namespace UniversityApp.CourseService.Repositories;

public interface ICourseRepository
{
	Task<IEnumerable<Course>> GetAllAsync(Specification<Course> specification);
	Task<Course?> GetByIdAsync(Guid id);
	Task AddAsync(Course course);
	Task UpdateAsync(Course course);
	Task DeleteAsync(Course course);
	Task<bool> DeleteByIdAsync(Guid id);
}