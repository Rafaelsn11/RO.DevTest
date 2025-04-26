using Microsoft.EntityFrameworkCore;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Persistence.Repositories;

public class TokenRepository : BaseRepository<RefreshToken>, ITokenRepository
{
    private readonly DefaultContext _context;

    public TokenRepository(DefaultContext context) : base(context)
    {
        _context = context;
    }

    public async Task<RefreshToken?> Get(string refreshToken, CancellationToken cancellationToken = default)
    {
        return await _context
            .RefreshTokens
            .AsNoTracking()
            .Include(token => token.User)
            .FirstOrDefaultAsync(token => token.Value.Equals(refreshToken), cancellationToken);
    }

    public async Task SaveNewRefreshToken(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        var tokens = await _context.RefreshTokens
            .Where(token => token.UserId == refreshToken.UserId)
            .ToListAsync(cancellationToken);

        _context.RefreshTokens.RemoveRange(tokens);

        await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
