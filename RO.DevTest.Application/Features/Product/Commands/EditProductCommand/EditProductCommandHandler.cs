using MediatR;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Domain.Exception;

namespace RO.DevTest.Application.Features.Product.Commands.EditProductCommand;

public class EditProductCommandHandler : IRequestHandler<EditProductCommand>
{
    private readonly IProductRepository _productRepository;

    public EditProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Handle(EditProductCommand request, CancellationToken cancellationToken)
    {
        await Validate(request, cancellationToken);
        var product = _productRepository.Get(x => x.Id == request.Id);
        
        if (product == null)
            throw new NotFoundException($"Produto com ID {request.Id} não encontrado");

        request.UpdateProduct(product);

        _productRepository.Update(product);
    }

    private async Task Validate(EditProductCommand request, CancellationToken cancellationToken)
    {
        EditProductCommandValidator validator = new();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new BadRequestException(validationResult);
    }
} 