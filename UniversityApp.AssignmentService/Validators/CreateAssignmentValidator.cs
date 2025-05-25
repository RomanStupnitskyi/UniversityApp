using FluentValidation;
using UniversityApp.Shared.DTOs;

namespace UniversityApp.AssignmentService.Validators;

public class CreateAssignmentValidator : AbstractValidator<CreateAssignmentDto>
{
	public CreateAssignmentValidator()
	{
		RuleFor(x => x.Id)
			.Must(id => Guid.TryParse(id, out _))
			.WithMessage("Id must be a valid GUID.")
			.Must(id => Guid.TryParse(id, out var parsed) && parsed != Guid.Empty)
			.WithMessage("Id must not be an empty GUID.");
		
		RuleFor(x => x.CourseId)
			.NotEmpty()
			.WithMessage("CourseId is required.")
			.Must(id => Guid.TryParse(id, out _))
			.WithMessage("CourseId must be a valid GUID.")
			.Must(id => Guid.TryParse(id, out var parsed) && parsed != Guid.Empty)
			.WithMessage("CourseId must not be an empty GUID.");
		
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