using CSharpFunctionalExtensions;
using UniversityApp.Shared.DTOs;
using UniversityApp.Shared.Models;

namespace UniversityApp.UserService.Services;

public interface ILecturerService
{
	Task<Result<IEnumerable<Lecturer>>> GetAllAsync();
	Task<Result<Lecturer>> GetByIdAsync(Guid id);
	Task<Result<Lecturer>> CreateAsync(CreateLecturerDto dto);
	Task<Result<Lecturer>> UpdateAsync(Guid id, UpdateLecturerDto dto);
	Task<Result> DeleteAsync(Guid id);
}