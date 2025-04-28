using System.ComponentModel.DataAnnotations;
using MediatR;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Domain.Exception;

namespace RO.DevTest.Application.Features.Product.Commands.CreateProductCommand;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreateProductResult>
{
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        await Validate(request, cancellationToken);
        var product = new Domain.Entities.Product
        {
            Name = request.Name,
            Price = request.Price,
            Stock = request.Stock
        };

        await _productRepository.CreateAsync(product, cancellationToken);

        return new CreateProductResult(product);
    }

    private async Task Validate(CreateProductCommand request, CancellationToken cancellationToken)
    {
        CreateProductCommandValidator validator = new();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new BadRequestException(validationResult);
    }
} 