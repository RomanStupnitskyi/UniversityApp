using CSharpFunctionalExtensions;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using UniversityApp.Shared.Models;
using UniversityApp.CourseService.Repositories;
using UniversityApp.CourseService.Specifications;
using UniversityApp.Shared.DTOs;
using UniversityApp.Shared.Events;
using UniversityApp.Shared.Queries;

namespace UniversityApp.CourseService.Services;

public class CourseService(
	ICourseRepository courseRepository,
	IPublishEndpoint publishEndpoint
	) : ICourseService
{
	public async Task<Result<IEnumerable<Course>>> GetAllAsync(CourseQuery query)
	{
		var specification = new CourseSpecification(query);
		
		var courses = await courseRepository.GetAllAsync(specification);
		return Result.Success(courses);
	}

	public async Task<Result<Course>> GetByIdAsync(Guid id)
	{
		var course = await courseRepository.GetByIdAsync(id);
		return course == null
			? Result.Failure<Course>($"Course with ID=\"{id}\" not found")
			: Result.Success(course);
	}

	public async Task<Result<Course>> CreateAsync(CreateCourseDto dto)
	{
		var course = new Course
		{
			Id = dto.Id,
			Title = dto.Title,
			Description = dto.Description,
			ECTS = dto.ECTS
		};
		
		await courseRepository.AddAsync(course);
		return Result.Success(course);
	}

	public async Task<Result<Course>> UpdateAsync(Guid id, UpdateCourseDto dto)
	{
		var course = await courseRepository.GetByIdAsync(id);
		if (course == null)
			return Result.Failure<Course>($"Course with ID=\"{id}\" not found");
		
		course.Title = dto.Title ?? course.Title;
		course.Description = dto.Description ?? course.Description;
		course.ECTS = dto.ECTS ?? course.ECTS;
		
		await courseRepository.UpdateAsync(course);
		return Result.Success(course);
	}

	public async Task<Result> DeleteAsync(Guid id)
	{
		var success = await courseRepository.DeleteByIdAsync(id);
		if (!success)
			return Result.Failure($"Course with ID=\"{id}\" not found");

		await publishEndpoint.Publish(new CourseDeletedEvent
		{
			CourseId = id
		});

		return Result.Success($"Course with ID=\"{id}\" deleted successfully");
	}
}