using Microsoft.AspNetCore.Mvc;
using UniversityApp.Shared.Models;
using UniversityApp.CourseService.Repositories;
using UniversityApp.Shared.DTOs;

namespace UniversityApp.CourseService.Services;

public class CourseService(ICourseRepository courseRepository) : ICourseService
{
	public async Task<ActionResult> GetAllAsync()
	{
		try
		{
			var courses = await courseRepository.GetAllAsync();
			return new OkObjectResult(courses);
		}
		catch (Exception)
		{
			return new ConflictObjectResult("Error occurred while fetching courses");
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
			
			var course = await courseRepository.GetByIdAsync(id);
			if (course == null)
			{
				return new NotFoundObjectResult($"Course with ID=\"{id}\" not found");
			}
			return new OkObjectResult(course);
		}
		catch (Exception)
		{
			return new ConflictObjectResult("Error occurred while fetching courses");
		}
	}

	public async Task<ActionResult> CreateAsync(CreateCourseDto dto)
	{
		try
		{
			var course = new Course
			{
				Id = dto.Id,
				Title = dto.Title,
				Description = dto.Description,
				ECTS = dto.ECTS
			};
			
			if (dto.ECTS == 10) return new ConflictObjectResult("Course with 10 ECTS is unavailable");
			
			var success = await courseRepository.AddAsync(course);
			return success
				? new OkObjectResult(course)
				: new ConflictObjectResult("Error occurred while creating course");
		}
		catch (Exception)
		{
			return new ConflictObjectResult("Error occurred while creating course");
		}
	}

	public async Task<ActionResult> UpdateAsync(string id, UpdateCourseDto dto)
	{
		try
		{
			var guid = Guid.TryParse(id, out var parsedId);
			if (!guid)
				return new BadRequestObjectResult("Invalid ID format");
			if (parsedId == Guid.Empty)
				return new BadRequestObjectResult("ID cannot be empty");
			
			var course = await courseRepository.GetByIdAsync(id);
			if (course == null)
				return new NotFoundObjectResult($"Course with ID=\"{id}\" not found");
			
			course.Title = dto.Title ?? course.Title;
			course.Description = dto.Description ?? course.Description;
			course.ECTS = dto.ECTS ?? course.ECTS;
			
			var success = await courseRepository.UpdateAsync(course);
			return success
				? new OkObjectResult(course)
				: new ConflictObjectResult("Error occurred while updating course");
		}
		catch (Exception)
		{
			return new ConflictObjectResult("Error occurred while updating course");
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
			
			var success = await courseRepository.DeleteByIdAsync(id);
			return success
				? new OkObjectResult($"Course with ID=\"{id}\" deleted successfully")
				: new ConflictObjectResult("Failed to delete course");
		}
		catch (KeyNotFoundException)
		{
			return new NotFoundObjectResult($"Course with ID=\"{id}\" not found");
		}
		catch (Exception)
		{
			return new ConflictObjectResult("Failed to delete course");
		}
	}
}