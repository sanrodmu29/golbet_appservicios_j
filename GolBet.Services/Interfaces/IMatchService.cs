using GolBet.Entities.Enums;
using GolBet.Services.DTOs;

namespace GolBet.Services.Interfaces;

public interface IMatchService
{
    /// <summary>Match board: all active matches ordered by date, optionally filtered by status.</summary>
    Task<IEnumerable<MatchDto>> GetBoardAsync(MatchStatus? status = null);

    /// <summary>Single match with teams and bets. Null when it does not exist.</summary>
    Task<MatchDetailDto?> GetDetailAsync(int id);
}
