using FluentValidation;
using UniversityApp.Shared.DTOs;

namespace UniversityApp.UserService.Validators;

public class CreateLecturerValidator : AbstractValidator<CreateLecturerDto>
{
	public CreateLecturerValidator()
	{
		RuleFor(x => x.Id)
			.NotEmpty()
			.WithMessage("Id is required.")
			.Must(id => id != Guid.Empty)
			.WithMessage("Id must not be an empty GUID.");

		RuleFor(x => x.ContactEmail)
			.EmailAddress()
			.WithMessage("Email must be a valid email address.");

		RuleFor(x => x.ContactNumber)
			.Matches(@"(?<!\w)(\(?(\+|00)?48\)?)?[ -]?\d{3}[ -]?\d{3}[ -]?\d{3}(?!\w)")
			.WithMessage("Contact number must be a valid phone number.")
			.MinimumLength(9)
			.WithMessage("Contact number must be at least 9 numbers.")
			.MaximumLength(11)
			.WithMessage("Contact number must be less than 11 numbers.");
	}
}