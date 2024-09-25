using FluentValidation;
using TechnicalTest.Shared.Models.Contacts;
using static TechnicalTest.Domain.Configurations.AppConstants;

namespace TechnicalTest.Application.Validators
{
    public class ContactDeleteRequestValidator : AbstractValidator<ContactDeleteRequest>
    {
        public ContactDeleteRequestValidator()
        {
            //Rules
            RuleFor(x => x.Id).NotNull().WithMessage(ValidationMessage.RequiredMessage)
                .GreaterThan(0).WithMessage(ValidationMessage.RequiredMessage);
        }
    }
}
