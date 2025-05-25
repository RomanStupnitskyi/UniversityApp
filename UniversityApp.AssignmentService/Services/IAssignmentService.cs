using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using UniversityApp.Shared.DTOs;
using UniversityApp.Shared.Models;

namespace UniversityApp.AssignmentService.Services;

public interface IAssignmentService
{
	Task<Result<IEnumerable<Assignment>>> GetAllAsync();
	Task<Result<Assignment>> GetByIdAsync(Guid id);
	Task<Result<Assignment>> CreateAsync(CreateAssignmentDto dto);
	Task<Result<Assignment>> UpdateAsync(Guid id, UpdateAssignmentDto dto);
	Task<Result> DeleteAsync(Guid id);
}