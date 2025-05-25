using FluentValidation;
using UniversityApp.Shared.DTOs;

// ReSharper disable ClassNeverInstantiated.Global

namespace UniversityApp.UserService.Validators;

public sealed class CreateStudentValidator : AbstractValidator<CreateStudentDto>
{
	public CreateStudentValidator()
	{
		RuleFor(x => x.Id)
			.NotEmpty()
			.WithMessage("Id is required.")
			.Must(id => Guid.TryParse(id, out _))
			.WithMessage("Id must be a valid GUID.")
			.Must(id => Guid.TryParse(id, out var parsed) && parsed != Guid.Empty)
			.WithMessage("Id must not be an empty GUID.");
		
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
