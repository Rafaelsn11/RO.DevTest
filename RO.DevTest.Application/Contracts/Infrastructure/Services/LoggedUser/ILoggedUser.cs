using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Application.Contracts.Infrastructure.Services.LoggedUser;

public interface ILoggedUser
{
    public Task<User> User();
}
