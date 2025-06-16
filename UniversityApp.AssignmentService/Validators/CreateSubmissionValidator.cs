using FluentValidation;
using UniversityApp.Shared.DTOs;

namespace UniversityApp.AssignmentService.Validators;

public class CreateSubmissionValidator : AbstractValidator<CreateSubmissionDto>
{
	public CreateSubmissionValidator()
	{
		RuleFor(x => x.Id)
			.Must(id => id != Guid.Empty)
			.WithMessage("CourseId must not be an empty GUID.");

		RuleFor(x => x.Content)
			.NotEmpty()
			.WithMessage("Submission content is required.")
			.MaximumLength(500)
			.WithMessage("Submission content cannot exceed 500 characters.");
	}
}