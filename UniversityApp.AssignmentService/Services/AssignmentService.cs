using Microsoft.AspNetCore.Mvc;
using UniversityApp.AssignmentService.API;
using UniversityApp.AssignmentService.Repositories;
using UniversityApp.Shared.DTOs;
using UniversityApp.Shared.Models;

namespace UniversityApp.AssignmentService.Services;

public class AssignmentService(
	IAssignmentRepository assignmentRepository,
	ICourseAPI courseAPI
	) : IAssignmentService
{
	public async Task<ActionResult> GetAllAsync()
	{
		try
		{
			var assignments = await assignmentRepository.GetAllAsync();
			return new OkObjectResult(assignments);
		}
		catch (Exception)
		{
			return new ConflictObjectResult("Failed to fetch assignments");
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

			var assignment = await assignmentRepository.GetByIdAsync(id);
			if (assignment == null)
				return new NotFoundObjectResult($"Assignment with ID=\"{id}\" not found");
			
			return new OkObjectResult(assignment);
		}
		catch (Exception)
		{
			return new ConflictObjectResult("Failed to fetch assignment");
		}
	}

	public async Task<ActionResult> CreateAsync(CreateAssignmentDto dto)
	{
		try
		{
			var assignment = await assignmentRepository.GetByIdAsync(dto.Id);
			if (assignment != null)
				return new NotFoundObjectResult($"Assignment with ID {dto.Id} already exists");
			
			var response = await courseAPI.GetCourseByIdAsync(dto.CourseId);
			if (!response.IsSuccessStatusCode)
				return new NotFoundObjectResult($"Course with ID {dto.CourseId} not found");
			
			var newAssignment = new Assignment
			{
				Id = dto.Id,
				CourseId = dto.CourseId,
				Title = dto.Title,
				Description = dto.Description,
				StartDate = dto.StartDate,
				EndDate = dto.EndDate
			};

			var success = await assignmentRepository.AddAsync(newAssignment);
			return success
				? new OkObjectResult(newAssignment)
				: new ConflictObjectResult("Failed to create assignment");
		}
		catch (Exception)
		{
			return new ConflictObjectResult("Failed to create assignment");
		}
	}

	public async Task<ActionResult> UpdateAsync(string id, UpdateAssignmentDto dto)
	{
		try
		{
			var guid = Guid.TryParse(id, out var parsedId);
			if (!guid)
				return new BadRequestObjectResult("Invalid ID format");
			if (parsedId == Guid.Empty)
				return new BadRequestObjectResult("ID cannot be empty");

			var assignment = await assignmentRepository.GetByIdAsync(id);
			if (assignment == null)
				return new NotFoundObjectResult($"Assignment with ID=\"{id}\" not found");

			switch (dto)
			{
				case { EndDate: not null, StartDate: null } when dto.EndDate <= assignment.StartDate:
					return new BadRequestObjectResult("End date must be after start date");
				case { StartDate: not null, EndDate: null } when dto.StartDate >= assignment.EndDate:
					return new BadRequestObjectResult("Start date must be before end date");
			}

			assignment.Title = dto.Title ?? assignment.Title;
			assignment.Description = dto.Description ?? assignment.Description;
			assignment.StartDate = dto.StartDate ?? assignment.StartDate;
			assignment.EndDate = dto.EndDate ?? assignment.EndDate;

			var success = await assignmentRepository.UpdateAsync(assignment);
			return success
				? new OkObjectResult(assignment)
				: new ConflictObjectResult("Failed to update assignment");
		}
		catch (Exception)
		{
			return new ConflictObjectResult("Failed to update assignment");
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

			var assignment = await assignmentRepository.GetByIdAsync(id);
			if (assignment == null)
				return new NotFoundObjectResult($"Assignment with ID=\"{id}\" not found");

			var success = await assignmentRepository.DeleteByIdAsync(id);
			return success
				? new OkObjectResult($"Assignment with ID=\"{id}\" deleted successfully")
				: new ConflictObjectResult("Failed to delete assignment");
		}
		catch (KeyNotFoundException)
		{
			return new NotFoundObjectResult($"Assignment with ID=\"{id}\" not found");
		}
		catch (Exception)
		{
			return new ConflictObjectResult("Failed to delete assignment");
		}
	}
}