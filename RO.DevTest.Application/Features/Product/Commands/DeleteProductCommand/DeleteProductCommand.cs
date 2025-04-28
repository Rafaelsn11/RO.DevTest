using MediatR;

namespace RO.DevTest.Application.Features.Product.Commands.DeleteProductCommand;

public class DeleteProductCommand : IRequest
{
    public Guid Id { get; set; }
} 