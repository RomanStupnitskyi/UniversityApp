using FluentValidation;
using UniversityApp.Shared.DTOs;

namespace UniversityApp.UserService.Validators;

public class UpdateStudentValidator : AbstractValidator<UpdateStudentDto>
{
	public UpdateStudentValidator()
	{
		RuleFor(x => x.StudentNumber)
			.NotEmpty()
			.WithMessage("StudentNumber is required.")
			.Matches("^[0-9]+$")
			.WithMessage("StudentNumber must contain only digits.")
			.MinimumLength(6)
			.WithMessage("StudentNumber must be at least 6 characters long.")
			.MaximumLength(12)
			.WithMessage("StudentNumber must be at most 12 characters long.");
	}
}