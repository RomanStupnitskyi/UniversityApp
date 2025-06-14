using CSharpFunctionalExtensions;
using UniversityApp.Shared.DTOs;
using UniversityApp.Shared.Models;
using UniversityApp.Shared.Queries;

namespace UniversityApp.CourseService.Services;

public interface ICourseService
{
	Task<Result<IEnumerable<Course>>> GetAllAsync(CourseQuery query);
	Task<Result<Course>> GetByIdAsync(Guid id);
	Task<Result<Course>> CreateAsync(CreateCourseDto dto);
	Task<Result<Course>> UpdateAsync(Guid id, UpdateCourseDto dto);
	Task<Result> DeleteAsync(Guid id);
}