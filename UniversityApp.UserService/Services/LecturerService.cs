using Microsoft.AspNetCore.Mvc;
using UniversityApp.Shared.DTOs;
using UniversityApp.Shared.Models;
using UniversityApp.UserService.Repositories;

namespace UniversityApp.UserService.Services;

public class LecturerService(ILecturerRepository lecturerRepository) : ILecturerService
{
	public async Task<ActionResult> GetAllAsync()
	{
		try
		{
			var students = await lecturerRepository.GetAllAsync();
			return new OkObjectResult(students);
		}
		catch (Exception)
		{
			return new ConflictObjectResult("Failed to retrieve lecturers.");
		}
	}

	public async Task<ActionResult> GetByIdAsync(string id)
	{
		try
		{
			var guid = Guid.TryParse(id, out var parsedId);
			if (!guid)
				return new BadRequestObjectResult("Invalid ID format");
			if (parsedId == Guid.Empty)
				return new BadRequestObjectResult("ID cannot be empty");
			
			var lecturer = await lecturerRepository.GetByIdAsync(id);
			return lecturer == null
				? new NotFoundObjectResult($"Lecturer with ID=\"{id}\" not found")
				: new OkObjectResult(lecturer);
		}
		catch (Exception)
		{
			return new ConflictObjectResult("Failed to retrieve lecturer.");
		}
	}

	public async Task<ActionResult> CreateAsync(CreateLecturerDto dto)
	{
		var lecturer = new Lecturer
		{
			Id = dto.Id,
			ContactEmail = dto.ContactEmail,
			ContactNumber = dto.ContactNumber,
			CreatedAt = DateTime.UtcNow
		};
		
		var existingLecturer = await lecturerRepository.GetByIdAsync(dto.Id);
		if (existingLecturer != null)
			return new ConflictObjectResult($"Lecturer with ID=\"{existingLecturer.Id}\" already exists.");

		var success = await lecturerRepository.AddAsync(lecturer);
		return success ? new OkObjectResult(lecturer) : new ConflictObjectResult("Failed to add lecturer.");
	}

	public async Task<ActionResult> UpdateAsync(string id, UpdateLecturerDto dto)
	{
		try
		{
			var guid = Guid.TryParse(id, out var parsedId);
			if (!guid)
				return new BadRequestObjectResult("Invalid ID format");
			if (parsedId == Guid.Empty)
				return new BadRequestObjectResult("ID cannot be empty");
			
			var lecturer = await lecturerRepository.GetByIdAsync(id);
			if (lecturer == null)
				return new NotFoundObjectResult($"Lecturer with ID=\"{id}\" not found");

			lecturer.ContactEmail = dto.ContactEmail;
			lecturer.ContactNumber = dto.ContactNumber;
			var success = await lecturerRepository.UpdateAsync(lecturer);
			return success ? new OkObjectResult(true) : new ConflictObjectResult("Failed to update student.");
		}
		catch (Exception)
		{
			return new ConflictObjectResult("Failed to update lecturer.");
		}
	}

	public async Task<ActionResult> DeleteAsync(string id)
	{
		try
		{
			var guid = Guid.TryParse(id, out var parsedId);
			if (!guid)
				return new BadRequestObjectResult("Invalid ID format");
			if (parsedId == Guid.Empty)
				return new BadRequestObjectResult("ID cannot be empty");
			
			var success = await lecturerRepository.DeleteByIdAsync(id);
			return success
				? new OkObjectResult(true)
				: new ConflictObjectResult("Failed to delete lecturer.");
		}
		catch (KeyNotFoundException)
		{
			return new NotFoundObjectResult($"Lecturer with ID=\"{id}\" not found");
		}
		catch (Exception)
		{
			return new ConflictObjectResult("Failed to delete lecturer.");
		}
	}
}