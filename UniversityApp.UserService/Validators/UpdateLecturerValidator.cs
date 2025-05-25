using FluentValidation;
using UniversityApp.Shared.DTOs;

namespace UniversityApp.UserService.Validators;

public class UpdateLecturerValidator : AbstractValidator<UpdateLecturerDto>
{
	public UpdateLecturerValidator()
	{
		RuleFor(x => x.ContactEmail)
			.EmailAddress()
			.WithMessage("Email must be a valid email address.");

		RuleFor(x => x.ContactNumber)
			.Matches(@"^(?:(?:\+|00)?48)?[ -]?\d{3}[ -]?\d{3}[ -]?\d{3}$")
			.WithMessage("Contact number must be a valid Polish phone number.")
			.MinimumLength(9)
			.WithMessage("Contact number must be at least 9 characters.")
			.MaximumLength(20)
			.WithMessage("Contact number must be less than 20 characters.");
	}
}