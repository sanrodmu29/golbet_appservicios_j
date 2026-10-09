using AutoMapper;
using GolBet.Entities;
using GolBet.Services.DTOs;

namespace GolBet.Services.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Flattening by convention:
        // MatchDto.HomeTeamName <- Match.HomeTeam.Name
        // MatchDto.AwayTeamCrestUrl <- Match.AwayTeam.CrestUrl
        CreateMap<Match, MatchDto>();

        // Detail (Module 5): inherits the board fields, adds TotalBets explicitly
        CreateMap<Match, MatchDetailDto>()
            .ForMember(dto => dto.TotalBets,
                       options => options.MapFrom(match => match.Bets.Count));
    }
}
