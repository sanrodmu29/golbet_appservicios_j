using AutoMapper;
using GolBet.Entities;
using GolBet.Repositories.Interfaces;
using GolBet.Services.DTOs;
using GolBet.Services.Interfaces;

namespace GolBet.Services.Implementations;

public class TeamService : ITeamService
{
    private readonly IGenericRepository<Team> _teamRepository;
    private readonly IMapper _mapper;

    public TeamService(IGenericRepository<Team> teamRepository, IMapper mapper)
    {
        _teamRepository = teamRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TeamDto>> GetAllAsync()
        => _mapper.Map<IEnumerable<TeamDto>>(await _teamRepository.GetAllAsync());

    public async Task<TeamFormDto?> GetForEditAsync(int id)
    {
        var team = await _teamRepository.GetByIdAsync(id);
        return team is null ? null : _mapper.Map<TeamFormDto>(team);
    }

    public async Task CreateAsync(TeamFormDto dto)
    {
        await EnsureUniqueNameAsync(dto);
        await _teamRepository.AddAsync(_mapper.Map<Team>(dto));
    }

    public async Task UpdateAsync(TeamFormDto dto)
    {
        await EnsureUniqueNameAsync(dto);
        var team = await _teamRepository.GetByIdAsync(dto.Id)
            ?? throw new KeyNotFoundException($"Team {dto.Id} not found.");
        _mapper.Map(dto, team);
        await _teamRepository.UpdateAsync(team);
    }

    public async Task DeactivateAsync(int id)
        => await _teamRepository.DeactivateAsync(id);

    // Team names are unique in the DB; checking here gives a friendly message instead of a SQL error.
    private async Task EnsureUniqueNameAsync(TeamFormDto dto)
    {
        var name = dto.Name.Trim();
        var all = await _teamRepository.GetAllAsync(includeInactive: true);
        if (all.Any(t => t.Id != dto.Id && string.Equals(t.Name.Trim(), name, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException($"Ya existe un equipo llamado «{name}».");
    }
}
