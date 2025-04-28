using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RO.DevTest.Application.Contracts.Infrastructure;
using RO.DevTest.Application.Contracts.Infrastructure.Services.LoggedUser;
using RO.DevTest.Domain.Exception;

namespace RO.DevTest.Application.Features.User.Commands.DeleteUserCommand;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IIdentityAbstractor _identityAbstractor;
    private readonly ILoggedUser _loggedUser;

    public DeleteUserCommandHandler(IIdentityAbstractor identityAbstractor, ILoggedUser loggedUser)
    {
        _identityAbstractor = identityAbstractor;
        _loggedUser = loggedUser;
    }

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var loggedUser = await _loggedUser.User();
        
        var user = await _identityAbstractor.FindUserByIdAsync(loggedUser.Id);
        if (user is null)
            throw new NotFoundException("Usuário não encontrado");

        var result = await _identityAbstractor.DeleteUser(user);
        if (!result.Succeeded)
            throw new BadRequestException(result);
    }
} 