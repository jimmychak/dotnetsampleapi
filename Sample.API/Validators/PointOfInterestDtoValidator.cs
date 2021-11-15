using FluentValidation;
using Sample.API.Models;

namespace Sample.API.Validators
{
    public class PointOfInterestDtoValidator : AbstractValidator<PointOfInterestDto>
    {
        public PointOfInterestDtoValidator()
        {
            RuleFor(p => p.Name)
                .MaximumLength(50)
                .Matches("^[a-zA-Z ]*$");

            RuleFor(p => p.Description)
                .MaximumLength(200);

            When(p => !string.IsNullOrEmpty(p.Description), () =>
            {
                RuleFor(p => p.Name).NotEmpty();
            });
        }
    }
}
