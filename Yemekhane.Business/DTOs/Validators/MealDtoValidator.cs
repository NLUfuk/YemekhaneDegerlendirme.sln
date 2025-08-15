using FluentValidation;

namespace Yemekhane.Business.DTOs.Validators;

public class MealDtoValidator : AbstractValidator<MealDto>
{
    public MealDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Yemek adı boş olamaz.");

        RuleFor(x => x.Name)
            .MaximumLength(100).WithMessage("Yemek adı 100 karakterden uzun olamaz.");

        RuleFor(x => x.Date)
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("Tarih bugünden eski olamaz.");
    }
}

