using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using UniversityApp.AssignmentService.API;
using UniversityApp.AssignmentService.Repositories;
using UniversityApp.AssignmentService.Specifications;
using UniversityApp.Shared.DTOs;
using UniversityApp.Shared.Events;
using UniversityApp.Shared.Models;
using UniversityApp.Shared.Queries;

namespace UniversityApp.AssignmentService.Services;

public class AssignmentService(
	IAssignmentRepository assignmentRepository,
	ICourseAPI courseApi,
	IPublishEndpoint publishEndpoint
	) : IAssignmentService
{
	public async Task<Result<IEnumerable<Assignment>>> GetAllAsync(AssignmentQuery query)
	{
		var specification = new AssignmentSpecification(query);
		
		var assignments = await assignmentRepository.GetAllAsync(specification);
		return Result.Success(assignments);
	}

	public async Task<Result<Assignment>> GetByIdAsync(Guid id)
	{
		var assignment = await assignmentRepository.GetByIdAsync(id);
		return assignment == null
			? Result.Failure<Assignment>($"Assignment with ID=\"{id}\" not found")
			: Result.Success(assignment);
	}

	[SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
	public async Task<Result<IEnumerable<Assignment>>> GetByCourseIdAsync(Guid id)
	{
		var assignments = await assignmentRepository.GetByCourseIdAsync(id);
		return !assignments.Any()
			? Result.Failure<IEnumerable<Assignment>>($"No assignments found for course ID=\"{id}\"")
			: Result.Success(assignments);
	}

	public async Task<Result<Assignment>> CreateAsync(CreateAssignmentDto dto)
	{
		var assignment = await assignmentRepository.GetByIdAsync(dto.Id);
		if (assignment != null)
			return Result.Failure<Assignment>($"Assignment with ID {dto.Id} already exists");
		
		var response = await courseApi.GetCourseByIdAsync(dto.CourseId);
		if (!response.IsSuccessStatusCode)
			return Result.Failure<Assignment>($"Course with ID {dto.CourseId} not found");
		
		var newAssignment = new Assignment
		{
			Id = dto.Id,
			CourseId = dto.CourseId,
			Title = dto.Title,
			Description = dto.Description,
			StartDate = dto.StartDate,
			EndDate = dto.EndDate
		};

		await assignmentRepository.AddAsync(newAssignment);
		return Result.Success(newAssignment);
	}

	public async Task<Result<Assignment>> UpdateAsync(Guid id, UpdateAssignmentDto dto)
	{
		var assignment = await assignmentRepository.GetByIdAsync(id);
		if (assignment == null)
			return Result.Failure<Assignment>($"Assignment with ID=\"{id}\" not found");

		switch (dto)
		{
			case { EndDate: not null, StartDate: null } when dto.EndDate <= assignment.StartDate:
				return Result.Failure<Assignment>("End date must be after start date");
			case { StartDate: not null, EndDate: null } when dto.StartDate >= assignment.EndDate:
				return Result.Failure<Assignment>("Start date must be before end date");
		}

		assignment.Title = dto.Title ?? assignment.Title;
		assignment.Description = dto.Description ?? assignment.Description;
		assignment.StartDate = dto.StartDate ?? assignment.StartDate;
		assignment.EndDate = dto.EndDate ?? assignment.EndDate;

		await assignmentRepository.UpdateAsync(assignment);
		return Result.Success(assignment);
	}

	[SuppressMessage("ReSharper", "ConvertIfStatementToReturnStatement")]
	public async Task<Result> DeleteAsync(Guid id)
	{
		var success = await assignmentRepository.DeleteByIdAsync(id);
		if (!success)
			return Result.Failure($"Assignment with ID=\"{id}\" not found");

		await publishEndpoint.Publish(new AssignmentDeletedEvent
		{
			AssignmentId = id
		});

		return Result.Success($"Assignment with ID=\"{id}\" deleted successfully");
	}

	public async Task<Result> DeleteByCourseIdAsync(Guid courseId)
	{
		var assignments = await assignmentRepository.GetByCourseIdAsync(courseId);
		if (!assignments.Any())
			return Result.Failure($"No assignments found for course ID=\"{courseId}\"");

		foreach (var assignment in assignments)
		{
			await assignmentRepository.DeleteAsync(assignment);
		}

		await publishEndpoint.Publish(new AssignmentsDeletedEvent
		{
			Assignments = assignments.ToList()
		});

		return Result.Success($"All assignments for course ID=\"{courseId}\" deleted successfully");
	}
}