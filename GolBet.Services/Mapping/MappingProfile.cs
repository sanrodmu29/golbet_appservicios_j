using AutoMapper;
using GolBet.Entities;
using GolBet.Services.DTOs;

namespace GolBet.Services.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Read models (flattening by convention: HomeTeamName <- HomeTeam.Name)
        CreateMap<Match, MatchDto>();
        CreateMap<Match, MatchDetailDto>()
            .ForMember(dto => dto.TotalBets,
                       options => options.MapFrom(match => match.Bets.Count));
        CreateMap<Team, TeamDto>();

        // Module 8: bet history. Flattens Match.HomeTeam.Name; PotentialPayout is a calculation.
        CreateMap<Bet, BetDto>()
            .ForMember(dto => dto.PotentialPayout,
                       options => options.MapFrom(bet => bet.Amount * bet.OddsAtPlacement));

        // Write models: both directions (save and load-for-edit)
        CreateMap<TeamFormDto, Team>().ReverseMap();
        CreateMap<MatchFormDto, Match>().ReverseMap();
    }
}
