using FluentValidation;
using TechnicalTest.Shared.Models.Contacts;
using static TechnicalTest.Domain.Configurations.AppConstants;

namespace TechnicalTest.Application.Validators
{
    public class ContactAddRequestValidator : AbstractValidator<ContactAddRequest>
    {
        public ContactAddRequestValidator()
        {
            //Rules
            RuleFor(x => x.Name).NotNull().WithMessage(ValidationMessage.RequiredMessage)
                .NotEmpty().WithMessage(ValidationMessage.RequiredMessage);
            RuleFor(x => x.PhoneNumber).NotNull().WithMessage(ValidationMessage.RequiredMessage)
                .NotEmpty().WithMessage(ValidationMessage.RequiredMessage);
            RuleFor(x => x.ContactType).NotNull().WithMessage(ValidationMessage.RequiredMessage)
                .NotEmpty().WithMessage(ValidationMessage.RequiredMessage);
            RuleFor(x => x.ContactGroupId).NotNull().WithMessage(ValidationMessage.RequiredMessage)
                .GreaterThan(0).WithMessage(ValidationMessage.RequiredMessage);
        }
    }
}
