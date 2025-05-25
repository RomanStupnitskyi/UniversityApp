using UniversityApp.Shared.Models;

namespace UniversityApp.UserService.Repositories;

public interface IStudentRepository
{
	Task<IEnumerable<Student>> GetAllAsync();
	Task<Student?> GetByIdAsync(Guid id);
	Task<Student?> FindStudentByStudentNumber(string id);
	Task AddAsync(Student student);
	Task UpdateAsync(Student student);
	Task DeleteAsync(Student student);
	Task<bool> DeleteByIdAsync(Guid id);
}