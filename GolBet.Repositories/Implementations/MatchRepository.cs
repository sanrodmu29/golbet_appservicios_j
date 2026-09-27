// GolBet.Repositories/Implementations/MatchRepository.cs
using GolBet.Entities;
using GolBet.Entities.Enums;
using GolBet.Repositories.Data;
using GolBet.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GolBet.Repositories.Implementations;

public class MatchRepository : GenericRepository<Match>, IMatchRepository // MATCH REPOSITORY ES UNA IMPLEMENTACION ESPECIFICA DE LA INTERFAZ IMatchRepository, HEREDANDO DE LA CLASE GENERICA GenericRepository<Match>
{
    public MatchRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Match>> GetAllWithTeamsAsync(MatchStatus? status = null)
    {
        var query = _dbSet
            .Include(m => m.HomeTeam) // SIRVE PARA HACER UN JOIN(SQL) CON LA ENTIDAD RELACIONADA HomeTeam, PERMITIENDO OBTENER LOS DETALLES DEL EQUIPO LOCAL JUNTO CON EL PARTIDO
            .Include(m => m.AwayTeam)
            .Where(m => m.IsActive)
            .AsNoTracking()
            .AsQueryable();

        if (status.HasValue)
            query = query.Where(m => m.Status == status.Value);

        return await query.OrderBy(m => m.Date).ToListAsync();// RETORNA UNA LISTA DE PARTIDOS ORDENADOS POR FECHA, CON LOS DETALLES DE LOS EQUIPOS INCLUIDOS, FILTRADOS POR ESTADO SI SE PROPORCIONA UNO
    }// ESTE METODO ES UTIL PARA OBTENER UNA LISTA DE PARTIDOS CON SUS EQUIPOS RELACIONADOS, POSIBILIDAD DE FILTRAR POR ESTADO Y ORDENADOS POR FECHA

    public async Task<Match?> GetByIdWithDetailsAsync(int id)
        => await _dbSet // BUSCA EL DBSET DE MATCHES, INCLUYE LOS DETALLES DE LOS EQUIPOS Y LAS APUESTAS RELACIONADAS, Y RETORNA EL PRIMER PARTIDO QUE COINCIDA CON EL ID PROPORCIONADO
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Bets)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id); // EQUIVALE AL TOLISTASYNC, PERO RETORNA EL PRIMER ELEMENTO QUE COINCIDA CON LA CONDICION, O NULL SI NO HAY COINCIDENCIAS
                 // SELECT * FROM Matches^^ WHERE Id = id
}
