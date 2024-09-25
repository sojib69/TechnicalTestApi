using FluentValidation;
using static TechnicalTest.Domain.Configurations.AppConstants;
using TechnicalTest.Shared.Models.Contacts;

namespace TechnicalTest.Application.Validators
{
    public class ContactEditRequestValidator : AbstractValidator<ContactEditRequest>
    {
        public ContactEditRequestValidator()
        {
            //Rules
            RuleFor(x => x.Id).NotNull().WithMessage(ValidationMessage.RequiredMessage)
                .GreaterThan(0).WithMessage(ValidationMessage.RequiredMessage);
            RuleFor(x => x.ContactType).NotNull().WithMessage(ValidationMessage.RequiredMessage)
                .NotEmpty().WithMessage(ValidationMessage.RequiredMessage);
        }
    }
}
