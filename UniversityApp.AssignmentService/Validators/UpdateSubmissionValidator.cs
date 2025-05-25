using FluentValidation;
using UniversityApp.Shared.DTOs;

namespace UniversityApp.AssignmentService.Validators;

public class UpdateSubmissionValidator : AbstractValidator<UpdateSubmissionDto>
{
	public UpdateSubmissionValidator()
	{
		RuleFor(x => x.Content)
			.NotEmpty()
			.WithMessage("Submission cannot be empty.")
			.MaximumLength(500)
			.WithMessage("Submission cannot exceed 500 characters.");
	}
}