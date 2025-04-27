using FluentValidation;

namespace RO.DevTest.Application.Features.User.Commands.ChangePasswordUserCommand;

public class ChangePasswordUserCommandValidator : AbstractValidator<ChangePasswordUserCommand> {
    public ChangePasswordUserCommandValidator() {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty()
            .WithMessage("A senha atual é obrigatória");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage("A nova senha é obrigatória")
            .MinimumLength(6)
            .WithMessage("A senha deve ter pelo menos 6 caracteres");

        RuleFor(x => x.ConfirmNewPassword)
            .NotEmpty()
            .WithMessage("A confirmação da senha é obrigatória")
            .Equal(x => x.NewPassword)
            .WithMessage("As senhas não coincidem");
    }
} 