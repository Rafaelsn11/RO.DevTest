namespace RO.DevTest.Application.Features.Product.Commands.CreateProductCommand;

public class CreateProductResult
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }

    public CreateProductResult() { }

    public CreateProductResult(Domain.Entities.Product product) 
    {
        Id = product.Id;
        Name = product.Name;
        Price = product.Price;
    }
}
