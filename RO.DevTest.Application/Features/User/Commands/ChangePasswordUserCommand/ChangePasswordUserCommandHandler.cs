using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RO.DevTest.Application.Contracts.Infrastructure;
using RO.DevTest.Application.Contracts.Infrastructure.Services.LoggedUser;
using RO.DevTest.Domain.Exception;

namespace RO.DevTest.Application.Features.User.Commands.ChangePasswordUserCommand;

public class ChangePasswordUserCommandHandler : IRequestHandler<ChangePasswordUserCommand> {
    private readonly IIdentityAbstractor _identityAbstractor;
    private readonly ILoggedUser _loggedUser;

    public ChangePasswordUserCommandHandler(IIdentityAbstractor identityAbstractor, ILoggedUser loggedUser) 
    {
        _identityAbstractor = identityAbstractor;
        _loggedUser = loggedUser;
    }

    public async Task Handle(ChangePasswordUserCommand request, CancellationToken cancellationToken) {
        await Validate(request, cancellationToken);

        var loggedUser = await _loggedUser.User();
        
        var user = await _identityAbstractor.FindUserByIdAsync(loggedUser.Id);
        if (user is null)
            throw new NotFoundException("Usuário não encontrado");

        IdentityResult changePasswordResult = await _identityAbstractor.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        
        if (!changePasswordResult.Succeeded)
            throw new BadRequestException(changePasswordResult);
    }

    private async Task Validate(ChangePasswordUserCommand request, CancellationToken cancellationToken) {
        ChangePasswordUserCommandValidator validator = new();
        ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new BadRequestException(validationResult);
    }
} 