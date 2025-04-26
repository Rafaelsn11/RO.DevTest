using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Application.Contracts.Persistance.Repositories;

public interface ITokenRepository : IBaseRepository<RefreshToken>
{
    Task<RefreshToken?> Get(string refreshToken, CancellationToken cancellationToken = default);
    Task SaveNewRefreshToken(RefreshToken refreshToken, CancellationToken cancellationToken = default);
}
