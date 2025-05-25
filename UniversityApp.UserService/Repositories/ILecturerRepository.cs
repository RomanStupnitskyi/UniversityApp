using UniversityApp.Shared.Models;

namespace UniversityApp.UserService.Repositories;

public interface ILecturerRepository
{
	Task<IEnumerable<Lecturer>> GetAllAsync();
	Task<Lecturer?> GetByIdAsync(string id);
	Task<bool> AddAsync(Lecturer lecturer);
	Task<bool> UpdateAsync(Lecturer lecturer);
	Task DeleteAsync(Lecturer lecturer);
	Task<bool> DeleteByIdAsync(string id);
}