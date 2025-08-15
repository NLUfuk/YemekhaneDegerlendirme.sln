using FluentValidation;


namespace Yemekhane.Business.DTOs.Validators;

public class UserDtoValidator : AbstractValidator<UserDto>
{
    public UserDtoValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().MaximumLength(50);
       
        //RuleFor(x => x.PasswordHash).NotEmpty().MaximumLength(100);

        // _context.Users.FirstOrDefault(u => u.UserName == username && u.PasswordHash == passwordHash)
    }
}