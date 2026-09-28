// GolBet.Services/Interfaces/IMatchService.cs
using AutoMapper;
using GolBet.Entities.Enums;
using GolBet.Repositories.Interfaces;
using GolBet.Services.DTOs;

namespace GolBet.Services.Interfaces;

public interface IMatchService
{
    /// <summary>Match board: all active matches ordered by date.</summary>
    Task<IEnumerable<MatchDto>> GetBoardAsync(MatchStatus? status = null);
}
//  UNA INTEFAZ CONM UN UNICO METODO : GetBoardAsync, QUE RETORNA UNA LISTA DE MATCHDTO, FILTRADOS POR ESTADO SI SE PROPORCIONA UNO.
 //ESTE METODO ES UTIL PARA OBTENER LA INFORMACION DE LOS PARTIDOS EN EL FRONTEND, SIN NECESIDAD DE INCLUIR
//  LAS PROPIEDADES DE NAVEGACION (HOME TEAM, AWAY TEAM, BETS), SOLO LOS DATOS NECESARIOS PARA MOSTRAR EN LA TABLA DE PARTIDOS

