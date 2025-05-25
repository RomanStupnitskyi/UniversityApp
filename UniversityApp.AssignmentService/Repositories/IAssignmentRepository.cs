using UniversityApp.Shared.Models;

namespace UniversityApp.AssignmentService.Repositories;

public interface IAssignmentRepository
{
	Task<IEnumerable<Assignment>> GetAllAsync();
	Task<Assignment?> GetByIdAsync(string id);
	Task<bool> AddAsync(Assignment assignment);
	Task<bool> UpdateAsync(Assignment assignment);
	Task<bool> DeleteAsync(Assignment assignment);
	Task<bool> DeleteByIdAsync(string id);
}