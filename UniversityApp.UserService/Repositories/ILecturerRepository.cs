using UniversityApp.Shared.Models;
using UniversityApp.UserService.Specifications;

namespace UniversityApp.UserService.Repositories;

public interface ILecturerRepository
{
	Task<IEnumerable<Lecturer>> GetAllAsync(LecturerSpecification specification);
	Task<Lecturer?> GetByIdAsync(Guid id);
	Task AddAsync(Lecturer lecturer);
	Task UpdateAsync(Lecturer lecturer);
	Task DeleteAsync(Lecturer lecturer);
	Task<bool> DeleteByIdAsync(Guid id);
}