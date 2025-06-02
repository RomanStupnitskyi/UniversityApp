using UniversityApp.AssignmentService.Specifications;
using UniversityApp.Shared.Models;

namespace UniversityApp.AssignmentService.Repositories;

public interface IAssignmentRepository
{
	Task<IEnumerable<Assignment>> GetAllAsync(AssignmentSpecification specification);
	Task<Assignment?> GetByIdAsync(Guid id);
	Task<IEnumerable<Assignment>> GetByCourseIdAsync(Guid courseId);
	Task AddAsync(Assignment assignment);
	Task UpdateAsync(Assignment assignment);
	Task DeleteAsync(Assignment assignment);
	Task<bool> DeleteByCourseIdAsync(Guid courseId);
	Task<bool> DeleteByIdAsync(Guid id);
}