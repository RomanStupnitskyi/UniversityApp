using FluentValidation;
using UniversityApp.Shared.DTOs;

namespace UniversityApp.AssignmentService.Validators;

public class CreateSubmissionValidator : AbstractValidator<CreateSubmissionDto>
{
	public CreateSubmissionValidator()
	{
		RuleFor(x => x.Id)
			.Must(id => Guid.TryParse(id, out _))
			.WithMessage("CourseId must be a valid GUID.")
			.Must(id => Guid.TryParse(id, out var parsed) && parsed != Guid.Empty)
			.WithMessage("CourseId must not be an empty GUID.");

		RuleFor(x => x.StudentId)
			.NotEmpty()
			.WithMessage("CourseId is required.")
			.Must(id => Guid.TryParse(id, out _))
			.WithMessage("CourseId must be a valid GUID.")
			.Must(id => Guid.TryParse(id, out var parsed) && parsed != Guid.Empty)
			.WithMessage("CourseId must not be an empty GUID.");

		RuleFor(x => x.Content)
			.NotEmpty()
			.WithMessage("Submission content is required.")
			.MaximumLength(500)
			.WithMessage("Submission content cannot exceed 500 characters.");
	}
}