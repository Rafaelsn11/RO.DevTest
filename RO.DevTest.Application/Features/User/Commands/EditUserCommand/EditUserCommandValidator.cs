using FluentValidation;

namespace RO.DevTest.Application.Features.User.Commands.EditUserCommand;

public class EditUserCommandValidator : AbstractValidator<EditUserCommand> {
    public EditUserCommandValidator() {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("O ID do usuário é obrigatório");

        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("O nome de usuário é obrigatório")
            .MaximumLength(256)
            .WithMessage("O nome de usuário não pode exceder 256 caracteres");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("O nome é obrigatório");
    }
} 