using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using UniversityApp.Shared.DTOs;
using UniversityApp.Shared.Models;
using UniversityApp.Shared.Queries;

namespace UniversityApp.UserService.Services;

public interface IStudentService
{
	Task<Result<IEnumerable<Student>>> GetAllAsync(StudentQuery query);
	Task<Result<Student>> GetByIdAsync(Guid id);
	Task<Result<Student>> CreateAsync(CreateStudentDto dto);
	Task<Result<Student>> UpdateAsync(Guid id, UpdateStudentDto dto);
	Task<Result> DeleteAsync(Guid id);
}