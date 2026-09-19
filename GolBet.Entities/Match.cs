// GolBet.Entities/Match.cs
using System.ComponentModel.DataAnnotations.Schema;
using GolBet.Entities.Common;
using GolBet.Entities.Enums;

namespace GolBet.Entities;

public class Match : AuditableEntity // HEREDA DE AUDITABLEENTITY PARA TENER LOS CAMPOS DE AUDITORIA (CREATEDAT, UPDATEDAT, CREATEDBY, UPDATEDBY)
{
    public DateTime Date { get; set; }

    public MatchStatus Status { get; set; } = MatchStatus.Scheduled; // ES DE TIPO ENUM QUE SE CREÓ 
    // SI ME PARO EN MATCHSTATUS Y DOY F12 ME LLEVA HASTA ESE ENUM

    public int? HomeGoals { get; set; }// LOS GOLES SON NULLABLES PORQUE AL CREAR EL PARTIDO NO HAY GOLES TODAVIA, SOLO SE SABEN CUANDO EL PARTIDO TERMINA
    public int? AwayGoals { get; set; }//                       ^^^^^^

    [Column(TypeName = "decimal(5,2)")] // DECORADOR DE LA CUOTA DE LOCAL. DE TIPO DECIMAL(5,2) EN SQL, PARA QUE NO HAYA PROBLEMAS DE PRECISION CON LOS DECIMALES
    public decimal HomeOdds { get; set; }// ODDS: CUOTAS

    [Column(TypeName = "decimal(5,2)")]
    public decimal DrawOdds { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal AwayOdds { get; set; }

    // Two foreign keys to the same table (Team)
    public int HomeTeamId { get; set; }
    //                                     VVV  ESE NULL! NO SERÁ NULL PORQUE CUANDO SE CREA EL PARTIDO, EL EQUIPO LOCAL YA EXISTE, POR ESO SE PONE NULL! PARA QUE NO DE ERROR AL ACCEDER A ESE CAMPO
    public Team HomeTeam { get; set; } = null!;

    public int AwayTeamId { get; set; }
    public Team AwayTeam { get; set; } = null!;

    //Navigation Property 
    public ICollection<Bet> Bets { get; set; } = new List<Bet>(); // CUANDO ES ICOLLECTION ES PORQUE UN PARTIDO PUEDE TENER MUCHAS APUESTAS, PERO UNA APUESTA SOLO PERTENECE A UN PARTIDO, POR ESO ES UNA RELACION DE UNO A MUCHOS
}                                             // ^^^ ESE NEW LIST QUIERE DECIR QUE SE CREA UNA LISTA VACIA DE APUESTAS CUANDO SE CREA UN PARTIDO, PARA QUE NO SEA NULL Y NO DE ERROR AL ACCEDER A ELLA
