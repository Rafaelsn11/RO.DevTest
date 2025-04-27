using MediatR;

namespace RO.DevTest.Application.Features.User.Commands.EditUserCommand;

public class EditUserCommand : IRequest<EditUserResult> 
{
    public string UserName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public void UpdateUser(Domain.Entities.User user) {
        user.UserName = UserName;
        user.Name = Name;
    }
} 