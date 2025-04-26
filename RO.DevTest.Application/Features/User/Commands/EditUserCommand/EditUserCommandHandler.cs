using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RO.DevTest.Application.Contracts.Infrastructure;
using RO.DevTest.Domain.Entities;
using RO.DevTest.Domain.Exception;

namespace RO.DevTest.Application.Features.User.Commands.EditUserCommand;

public class EditUserCommandHandler : IRequestHandler<EditUserCommand, EditUserResult> {
    private readonly IIdentityAbstractor _identityAbstractor;

    public EditUserCommandHandler(IIdentityAbstractor identityAbstractor) {
        _identityAbstractor = identityAbstractor;
    }

    public async Task<EditUserResult> Handle(EditUserCommand request, CancellationToken cancellationToken) {
        await Validate(request, cancellationToken);

        var user = await _identityAbstractor.FindUserByIdAsync(request.Id);
        if (user is null)
            throw new NotFoundException("Usuário não encontrado");

        request.UpdateUser(user);
        IdentityResult updateResult = await _identityAbstractor.UpdateUserAsync(user);
        
        if (!updateResult.Succeeded)
            throw new BadRequestException(updateResult);

        return new EditUserResult(user);
    }

    private async Task Validate(EditUserCommand request, CancellationToken cancellationToken) {
        EditUserCommandValidator validator = new();
        ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new BadRequestException(validationResult);
    }
} 