﻿using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using UniversityApp.Shared.DTOs;
using UniversityApp.Shared.Models;
using UniversityApp.UserService.Repositories;

namespace UniversityApp.UserService.Services;

public class StudentService(IStudentRepository studentRepository) : IStudentService
{
	public async Task<Result<IEnumerable<Student>>> GetAllAsync()
	{
		var students = await studentRepository.GetAllAsync();
		
		return Result.Success(students);
	}

	public async Task<Result<Student>> GetByIdAsync(Guid id)
	{
		var student = await studentRepository.GetByIdAsync(id);
		return student == null
			? Result.Failure<Student>($"Student with ID=\"{id}\" not found")
			: Result.Success(student);
	}

	public async Task<Result<Student>> CreateAsync(CreateStudentDto dto)
	{
		var student = new Student
		{
			Id = dto.Id,
			StudentNumber = dto.StudentNumber,
			CreatedAt = DateTime.UtcNow
		};
		
		var existingStudent = await studentRepository.GetByIdAsync(dto.Id);
		if (existingStudent != null)
			return Result.Failure<Student>($"Student with ID=\"{existingStudent.Id}\" already exists.");
		
		var existingStudentWithSameNumber = await studentRepository.FindStudentByStudentNumber(dto.StudentNumber);
		return existingStudentWithSameNumber != null
			? Result.Failure<Student>(
				$"Student with StudentNumber=\"{existingStudentWithSameNumber.StudentNumber}\" already exists.")
			: Result.Success(student);
	}

	public async Task<Result<Student>> UpdateAsync(Guid id, UpdateStudentDto dto)
	{
		var student = await studentRepository.GetByIdAsync(id);
		if (student == null)
			return Result.Failure<Student>($"Student with ID=\"{id}\" not found");

		if (dto.StudentNumber != null && dto.StudentNumber != student.StudentNumber)
		{
			var existingStudentWithSameNumber = await studentRepository.FindStudentByStudentNumber(dto.StudentNumber);
			if (existingStudentWithSameNumber != null)
				return Result.Failure<Student>($"Student with Student Number \"{existingStudentWithSameNumber.StudentNumber}\" already exists.");
		}

		student.StudentNumber = dto.StudentNumber ?? student.StudentNumber;
		return Result.Success(student);
	}

	public async Task<Result> DeleteAsync(Guid id)
	{
		var success = await studentRepository.DeleteByIdAsync(id);
		return success
			? Result.Success()
			: Result.Failure($"Student with ID=\"{id}\" not found");
	}
}