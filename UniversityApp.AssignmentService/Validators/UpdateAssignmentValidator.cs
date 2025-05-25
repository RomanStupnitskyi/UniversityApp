using FluentValidation;
using UniversityApp.Shared.DTOs;

namespace UniversityApp.AssignmentService.Validators;

public class UpdateAssignmentValidator : AbstractValidator<UpdateAssignmentDto>
{
	public UpdateAssignmentValidator()
	{
		RuleFor(x => x.Title)
			.NotEmpty()
			.WithMessage("Title is required.")
			.MaximumLength(100)
			.WithMessage("Title must not exceed 100 characters.");

		RuleFor(x => x.Description)
			.MaximumLength(500)
			.WithMessage("Description must not exceed 500 characters.");

		RuleFor(x => x.StartDate)
			.GreaterThanOrEqualTo(DateTime.UtcNow)
			.WithMessage("Start date must be in the future.");

		RuleFor(x => x.EndDate)
			.GreaterThanOrEqualTo(x => x.StartDate)
			.WithMessage("End date must be greater than or equal to start date.");
	}
}