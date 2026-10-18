// GolBet.Repositories/Interfaces/IMatchRepository.cs
using GolBet.Entities;
using GolBet.Entities.Enums;

namespace GolBet.Repositories.Interfaces;

public interface IMatchRepository : IGenericRepository<Match> // NO TIENE RESTRICCION PORQUE ES MATCH
{
    Task<IEnumerable<Match>> GetAllWithTeamsAsync(MatchStatus? status = null, string? texto = null); // APLICA IMPLEMENTACION EN MATCH REPOSITORY PARA OBTENER TODOS LOS PARTIDOS CON SUS EQUIPOS, FILTRANDO POR ESTADO Y/O POR NOMBRE DE EQUIPO (LOCAL O VISITANTE) SI SE PROPORCIONAN
    Task<Match?> GetByIdWithDetailsAsync(int id);
}
