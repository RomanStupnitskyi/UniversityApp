using CSharpFunctionalExtensions;
using UniversityApp.Shared.DTOs;
using UniversityApp.Shared.Models;
using UniversityApp.Shared.Queries;

namespace UniversityApp.AssignmentService.Services;

public interface IAssignmentService
{
	Task<Result<IEnumerable<Assignment>>> GetAllAsync(AssignmentQuery query);
	Task<Result<Assignment>> GetByIdAsync(Guid id);
	Task<Result<IEnumerable<Assignment>>> GetByCourseIdAsync(Guid id);
	Task<Result<Assignment>> CreateAsync(CreateAssignmentDto dto);
	Task<Result<Assignment>> UpdateAsync(Guid id, UpdateAssignmentDto dto);
	Task<Result> DeleteAsync(Guid id);
	Task<Result> DeleteByCourseIdAsync(Guid courseId);
}