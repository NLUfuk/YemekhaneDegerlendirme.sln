using FluentValidation;

namespace Yemekhane.Business.DTOs.Validators;

public class RatingDtoValidator : AbstractValidator<RatingDto>
{


    public RatingDtoValidator()
    {
        RuleFor(x => x.Score)
            .InclusiveBetween(1, 5)
            .WithMessage("Puan 1-5 arası olmalı.");

        RuleFor(x => x.MealId)
            .GreaterThan(0)
            .WithMessage("MealId geçersiz.");

        RuleFor(x => x.Comment)
            .MaximumLength(500)
            .WithMessage("Yemek 500 karakter ile yorumlanacak kadar spesifik değil.");


        When(x => x.Score <= 2, () =>
        {
            RuleFor(x => x.Comment)
                .NotEmpty()
                .WithMessage("Düşük puanın sebebini açıklayınız!");
        });
    }
}
