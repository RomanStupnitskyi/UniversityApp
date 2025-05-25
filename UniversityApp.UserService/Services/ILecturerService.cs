using Microsoft.AspNetCore.Mvc;
using UniversityApp.Shared.DTOs;

namespace UniversityApp.UserService.Services;

public interface ILecturerService
{
	Task<ActionResult> GetAllAsync();
	Task<ActionResult> GetByIdAsync(string id);
	Task<ActionResult> CreateAsync(CreateLecturerDto dto);
	Task<ActionResult> UpdateAsync(string id, UpdateLecturerDto dto);
	Task<ActionResult> DeleteAsync(string id);
}