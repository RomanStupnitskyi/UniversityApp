using Microsoft.AspNetCore.Mvc;
using UniversityApp.Shared.DTOs;

namespace UniversityApp.UserService.Services;

public interface IStudentService
{
	Task<ActionResult> GetAllAsync();
	Task<ActionResult> GetByIdAsync(string id);
	Task<ActionResult> CreateAsync(CreateStudentDto dto);
	Task<ActionResult> UpdateAsync(string id, UpdateStudentDto dto);
	Task<ActionResult> DeleteAsync(string id);
}