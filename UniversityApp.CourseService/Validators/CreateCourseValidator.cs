using FluentValidation;
using UniversityApp.Shared.DTOs;

namespace UniversityApp.CourseService.Validators;

public class CreateCourseValidator : AbstractValidator<CreateCourseDto>
{
	public CreateCourseValidator()
	{
		RuleFor(x => x.Id)
			.Must(id => id != Guid.Empty)
			.WithMessage("Id must not be an empty GUID.");
			
		RuleFor(x => x.Title)
			.NotEmpty()
			.WithMessage("Title is required.")
			.MaximumLength(100)
			.WithMessage("Title must not exceed 100 characters.");

		RuleFor(x => x.Description)
			.MaximumLength(500)
			.WithMessage("Description must not exceed 500 characters.");

		RuleFor(x => x.ECTS)
			.NotEmpty()
			.WithMessage("ECTS is required.")
			.InclusiveBetween(1, 30)
			.WithMessage("ECTS must be a number between 1 and 30.");
	}
}