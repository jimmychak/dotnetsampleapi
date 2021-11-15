using FluentValidation;
using Sample.API.Models;

namespace Sample.API.Validators
{
    public class CityDtoValidator : AbstractValidator<CityDto>
    {
        public CityDtoValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .MaximumLength(50)
                .Matches("^[a-zA-Z ]*$");

            RuleFor(c => c.Description)
                .MaximumLength(200);

            RuleForEach(c => c.PointsOfInterest)
                .SetValidator(new PointOfInterestDtoValidator());
        }
    }
}
