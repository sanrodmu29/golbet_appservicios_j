// GolBet.Repositories/Interfaces/IMatchRepository.cs
using GolBet.Entities;
using GolBet.Entities.Enums;

namespace GolBet.Repositories.Interfaces;

public interface IMatchRepository : IGenericRepository<Match> // NO TIENE RESTRICCION PORQUE ES MATCH
{
    Task<IEnumerable<Match>> GetAllWithTeamsAsync(MatchStatus? status = null); // APLICA IMPLEMENTACION EN MATCH REPOSITORY PARA OBTENER TODOS LOS PARTIDOS CON SUS EQUIPOS, FILTRANDO POR ESTADO SI SE PROPORCIONA UNO
    Task<Match?> GetByIdWithDetailsAsync(int id);
}
