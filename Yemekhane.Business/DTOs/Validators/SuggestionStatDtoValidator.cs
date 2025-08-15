using FluentValidation;

namespace Yemekhane.Business.DTOs.Validators
{
    public class SuggestionStatDtoValidator : AbstractValidator<SuggestionStatDto>
    {
        public SuggestionStatDtoValidator()
        {
            RuleFor(x => x.MealAverageScore)
                .GreaterThan(5)
                .WithMessage("Sistemsel bir hata var. ");
            RuleFor(x => x.VoteCount)
                .LessThanOrEqualTo(0).WithMessage("Sistemsel bir hata var. ");
        }
    }
}
