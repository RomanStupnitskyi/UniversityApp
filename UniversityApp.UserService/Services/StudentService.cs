using Microsoft.AspNetCore.Mvc;
using UniversityApp.Shared.DTOs;
using UniversityApp.Shared.Models;
using UniversityApp.UserService.Repositories;

namespace UniversityApp.UserService.Services;

public class StudentService(IStudentRepository studentRepository) : IStudentService
{
	public async Task<ActionResult> GetAllAsync()
	{
		try
		{
			var students = await studentRepository.GetAllAsync();
			return new OkObjectResult(students);
		}
		catch (Exception)
		{
			return new ConflictObjectResult("Failed to retrieve students.");
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
			
			var student = await studentRepository.GetByIdAsync(id);
			return student == null
				? new NotFoundObjectResult($"Student with ID=\"{id}\" not found")
				: new OkObjectResult(student);
		}
		catch (Exception)
		{
			return new ConflictObjectResult("Failed to retrieve student.");
		}
	}

	public async Task<ActionResult> CreateAsync(CreateStudentDto dto)
	{
		var student = new Student
		{
			Id = dto.Id,
			StudentNumber = dto.StudentNumber,
			CreatedAt = DateTime.UtcNow
		};
		
		var existingStudent = await studentRepository.GetByIdAsync(dto.Id);
		if (existingStudent != null)
			return new ConflictObjectResult($"Student with ID=\"{existingStudent.Id}\" already exists.");
		
		var existingStudentWithSameNumber = await studentRepository.FindStudentByStudentNumber(dto.StudentNumber);
		if (existingStudentWithSameNumber != null)
			return new ConflictObjectResult($"Student with ID=\"{existingStudentWithSameNumber.Id}\" already exists.");

		var success = await studentRepository.AddAsync(student);
		return success ? new OkObjectResult(student) : new ConflictObjectResult("Failed to add student.");
	}

	public async Task<ActionResult> UpdateAsync(string id, UpdateStudentDto dto)
	{
		try
		{
			var guid = Guid.TryParse(id, out var parsedId);
			if (!guid)
				return new BadRequestObjectResult("Invalid ID format");
			if (parsedId == Guid.Empty)
				return new BadRequestObjectResult("ID cannot be empty");
			
			var student = await studentRepository.GetByIdAsync(id);
			if (student == null)
				return new NotFoundObjectResult($"Student with ID=\"{id}\" not found");

			if (dto.StudentNumber != null && dto.StudentNumber != student.StudentNumber)
			{
				var existingStudentWithSameNumber = await studentRepository.FindStudentByStudentNumber(dto.StudentNumber);
				if (existingStudentWithSameNumber != null)
					return new ConflictObjectResult($"Student with Student Number \"{existingStudentWithSameNumber.StudentNumber}\" already exists.");
			}

			student.StudentNumber = dto.StudentNumber ?? student.StudentNumber;
			var success = await studentRepository.UpdateAsync(student);
			return success ? new OkObjectResult(true) : new ConflictObjectResult("Failed to update student.");
		}
		catch (Exception)
		{
			return new ConflictObjectResult("Failed to update student.");
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
			
			var success = await studentRepository.DeleteByIdAsync(id);
			return success
				? new OkObjectResult(true)
				: new ConflictObjectResult("Failed to delete student.");
		}
		catch (KeyNotFoundException)
		{
			return new NotFoundObjectResult($"Student with ID=\"{id}\" not found");
		}
		catch (Exception)
		{
			return new ConflictObjectResult("Failed to delete student.");
		}
	}
}