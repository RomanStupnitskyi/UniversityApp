using Ardalis.Specification;
using CSharpFunctionalExtensions;
using UniversityApp.Shared.DTOs;
using UniversityApp.Shared.Models;
using UniversityApp.Shared.Queries;
using UniversityApp.UserService.Integrations.Services;
using UniversityApp.UserService.Repositories;
using UniversityApp.UserService.Specifications;

namespace UniversityApp.UserService.Services;

public class LecturerService(
	ILecturerRepository lecturerRepository,
	IKeycloakAdminService keycloakAdminService
	) : ILecturerService
{
	public async Task<Result<IEnumerable<Lecturer>>> GetAllAsync(LecturerQuery query)
	{
		var specification = new LecturerSpecification(query);
		
		var lecturers = await lecturerRepository.GetAllAsync(specification);
		return Result.Success(lecturers);
	}

	public async Task<Result<Lecturer>> GetByIdAsync(Guid id)
	{
		var lecturer = await lecturerRepository.GetByIdAsync(id);
		return lecturer == null
			? Result.Failure<Lecturer>($"Lecturer with ID=\"{id}\" not found")
			: Result.Success(lecturer);
	}

	public async Task<Result<Lecturer>> CreateAsync(CreateLecturerDto dto)
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
			return Result.Failure<Lecturer>($"Lecturer with ID=\"{existingLecturer.Id}\" already exists.");

		await lecturerRepository.AddAsync(lecturer);
		
		try
		{
			await keycloakAdminService.AssignRoleToUserAsync(lecturer.Id.ToString(), "lecturer");
		}
		catch (Exception ex)
		{
			return Result.Failure<Lecturer>($"Failed to assign role to lecturer: {ex.Message}");
		}
		
		return Result.Success(lecturer);
	}

	public async Task<Result<Lecturer>> UpdateAsync(Guid id, UpdateLecturerDto dto)
	{
		var lecturer = await lecturerRepository.GetByIdAsync(id);
		if (lecturer == null)
			return Result.Failure<Lecturer>($"Lecturer with ID=\"{id}\" not found");

		lecturer.ContactEmail = dto.ContactEmail;
		lecturer.ContactNumber = dto.ContactNumber;
		await lecturerRepository.UpdateAsync(lecturer);
		
		return Result.Success(lecturer);
	}

	public async Task<Result> DeleteAsync(Guid id)
	{
		var success = await lecturerRepository.DeleteByIdAsync(id);
		return success
			? Result.Success()
			: Result.Failure($"Lecturer with ID=\"{id}\" not found");
	}
}