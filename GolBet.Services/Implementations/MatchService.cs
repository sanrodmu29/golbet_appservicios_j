// GolBet.Services/Implementations/MatchService.cs
using AutoMapper;
using GolBet.Entities.Enums;
using GolBet.Repositories.Interfaces;
using GolBet.Services.DTOs;
using GolBet.Services.Interfaces;

namespace GolBet.Services.Implementations;

public class MatchService : IMatchService  // INYECCION DE DEPENDENCIAS: IMPLEMENTA LA INTERFAZ IMatchService, QUE DEFINE EL METODO GetBoardAsync, Y UTILIZA EL REPOSITORIO DE PARTIDOS (IMatchRepository) PARA OBTENER LOS DATOS DE LOS PARTIDOS Y MAPEARLOS A DTOs (MatchDto) PARA SU USO EN EL FRONTEND.
{
    // DEPENDENCIAS: ASIGNA TAREAS CON UN MISMO OBJETO VARIAS TAREAS, EN ESTE CASO EL REPOSITORIO DE PARTIDOS Y EL MAPEADOR DE OBJETOS. ESTO PERMITE QUE LA CLASE MatchService PUEDA UTILIZAR ESTAS DEPENDENCIAS PARA OBTENER LOS DATOS DE LOS PARTIDOS Y MAPEARLOS A DTOs PARA SU USO EN EL FRONTEND.
    private readonly IMatchRepository _matchRepository;  
    private readonly IMapper _mapper;

    public MatchService(IMatchRepository matchRepository, IMapper mapper)
    {
        _matchRepository = matchRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MatchDto>> GetBoardAsync(MatchStatus? status = null)
    {
        var matches = await _matchRepository.GetAllWithTeamsAsync(status);
        return _mapper.Map<IEnumerable<MatchDto>>(matches);//  De a entidad match a DTO MatchDto 

        // EN ESTE METODO TAMBIEN PUEDEN IR LOS RESULTADOS PARA FILTRAR POR PARTIDOS FINALIZADOS, EN CURSO O PROGRAMADOS, POR ESO EL PARAMETRO OPCIONAL DE STATUS

    }// LLAMA LA DEPENDENCIA DE MARCH REPOSITORY PARA OBTENER LOS PARTIDOS CON SUS EQUIPOS Y LUEGO LOS MAPEA A DTOs


}
