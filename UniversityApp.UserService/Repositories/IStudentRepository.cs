using UniversityApp.Shared.Models;

namespace UniversityApp.UserService.Repositories;

public interface IStudentRepository
{
	Task<IEnumerable<Student>> GetAllAsync();
	Task<Student?> GetByIdAsync(string id);
	Task<Student?> FindStudentByStudentNumber(string id);
	Task<bool> AddAsync(Student student);
	Task<bool> UpdateAsync(Student student);
	Task DeleteAsync(Student student);
	Task<bool> DeleteByIdAsync(string id);
}