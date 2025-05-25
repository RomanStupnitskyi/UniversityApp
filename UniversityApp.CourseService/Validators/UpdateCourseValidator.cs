using FluentValidation;
using UniversityApp.Shared.DTOs;

namespace UniversityApp.CourseService.Validators;

public class UpdateCourseValidator : AbstractValidator<UpdateCourseDto>
{
	public UpdateCourseValidator()
	{
		RuleFor(x => x.Title)
			.NotEmpty()
			.WithMessage("Title is required.")
			.MaximumLength(100)
			.WithMessage("Title must not exceed 100 characters.");

		RuleFor(x => x.Description)
			.MaximumLength(500)
			.WithMessage("Description must not exceed 500 characters.");

		RuleFor(x => x.ECTS)
			.InclusiveBetween(1, 30)
			.WithMessage("ECTS must be a number between 1 and 30.");
	}
}