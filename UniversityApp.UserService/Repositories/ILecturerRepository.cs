using UniversityApp.Shared.Models;

namespace UniversityApp.UserService.Repositories;

public interface ILecturerRepository
{
	Task<IEnumerable<Lecturer>> GetAllAsync();
	Task<Lecturer?> GetByIdAsync(Guid id);
	Task AddAsync(Lecturer lecturer);
	Task UpdateAsync(Lecturer lecturer);
	Task DeleteAsync(Lecturer lecturer);
	Task<bool> DeleteByIdAsync(Guid id);
}