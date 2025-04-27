using MediatR;

namespace RO.DevTest.Application.Features.User.Commands.ChangePasswordUserCommand;

public class ChangePasswordUserCommand : IRequest {
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmNewPassword { get; set; } = string.Empty;
} 