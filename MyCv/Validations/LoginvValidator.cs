using FluentValidation;

public class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(x=> x.Username)
            .NotNull()
            .WithMessage("Username is required")
            .NotEmpty();
        RuleFor(x=> x.Password)
            .NotNull()
            .WithMessage("Password is required")
            .NotEmpty();
    }
}