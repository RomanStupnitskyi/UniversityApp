using UniversityApp.Shared.Models;

namespace UniversityApp.AssignmentService.Repositories;

public interface IAssignmentRepository
{
	Task<IEnumerable<Assignment>> GetAllAsync();
	Task<Assignment?> GetByIdAsync(Guid id);
	Task AddAsync(Assignment assignment);
	Task UpdateAsync(Assignment assignment);
	Task DeleteAsync(Assignment assignment);
	Task<bool> DeleteByCourseIdAsync(Guid courseId);
	Task<bool> DeleteByIdAsync(Guid id);
}