using CSharpFunctionalExtensions;
using UniversityApp.Shared.DTOs;
using UniversityApp.Shared.Models;
using UniversityApp.UserService.Repositories;

namespace UniversityApp.UserService.Services;

public class LecturerService(ILecturerRepository lecturerRepository) : ILecturerService
{
	public async Task<Result<IEnumerable<Lecturer>>> GetAllAsync()
	{
		var lecturers = await lecturerRepository.GetAllAsync();
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