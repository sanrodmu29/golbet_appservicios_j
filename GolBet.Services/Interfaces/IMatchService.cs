using GolBet.Entities.Enums;
using GolBet.Services.DTOs;

namespace GolBet.Services.Interfaces;

public interface IMatchService
{
    // Reads (Modules 4 and 5)
    Task<IEnumerable<MatchDto>> GetBoardAsync(MatchStatus? status = null);
    Task<MatchDetailDto?> GetDetailAsync(int id);

    // Writes (Module 6)
    Task<MatchFormDto?> GetForEditAsync(int id);
    Task CreateAsync(MatchFormDto dto);
    Task UpdateAsync(MatchFormDto dto);
    Task DeactivateAsync(int id);
}
