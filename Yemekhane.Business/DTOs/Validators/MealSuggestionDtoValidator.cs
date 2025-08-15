
using FluentValidation;

namespace Yemekhane.Business.DTOs.Validators;

public class MealSuggestionDtoValidator : AbstractValidator<MealSuggestionDto>
{
    public MealSuggestionDtoValidator()
    {
        RuleFor(x => x.MealName)
            .NotEmpty().WithMessage("Yemek adı boş bırakılamaz!")
            .MaximumLength(100).WithMessage("Öyle bir yemek yok ");

        

    }
}
