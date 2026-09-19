// GolBet.Entities/Team.cs
using System.ComponentModel.DataAnnotations;
using GolBet.Entities.Common;

namespace GolBet.Entities;

public class Team : AuditableEntity
{
    [Required] // DECORADORES: Required = obligatorio, MaxLength = longitud máxima de caracteres
    [MaxLength(80)]  //        COMO SI FUERAN EL NVARCHAR(80) DE SQL Y EL REQUIRED ES COMO SI FUERA EL NOT NULL DE SQL
    public string Name { get; set; } = null!;

    [Required, MaxLength(60)]
    public string City { get; set; } = null!;

    [MaxLength(300)]
    public string? CrestUrl { get; set; }// ESCUDO DEL EQUIPO, OPCIONAL, POR ESO ES NULLABLE, YA QUE NO TODOS LOS EQUIPOS TIENEN ESCUDO

}
