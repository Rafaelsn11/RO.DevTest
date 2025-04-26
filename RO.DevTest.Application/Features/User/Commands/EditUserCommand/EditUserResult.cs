namespace RO.DevTest.Application.Features.User.Commands.EditUserCommand;

public record EditUserResult {
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public EditUserResult() { }

    public EditUserResult(Domain.Entities.User user) {
        Id = user.Id;
        UserName = user.UserName!;
        Name = user.Name;
    }
} 