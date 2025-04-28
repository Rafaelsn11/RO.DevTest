using FluentValidation;

namespace RO.DevTest.Application.Features.Product.Commands.EditProductCommand;

public class EditProductCommandValidator : AbstractValidator<EditProductCommand>
{
    public EditProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("O ID do produto é obrigatório");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("O nome do produto é obrigatório")
            .MaximumLength(100)
            .WithMessage("O nome do produto não pode exceder 100 caracteres");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("O preço deve ser maior que zero");

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0)
            .WithMessage("O estoque deve ser maior ou igual a zero");
    }
} 