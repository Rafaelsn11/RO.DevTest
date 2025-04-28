using MediatR;

namespace RO.DevTest.Application.Features.Product.Commands.EditProductCommand;

public class EditProductCommand : IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }

    public EditProductCommand() {}

    public void UpdateProduct(Domain.Entities.Product product)
    {
        Id = product.Id;
        Name = product.Name;
        Price = product.Price;
        Stock = product.Stock;
    }
} 