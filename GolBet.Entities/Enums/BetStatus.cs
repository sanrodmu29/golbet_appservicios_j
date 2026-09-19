// GolBet.Entities/Enums/BetStatus.cs
namespace GolBet.Entities.Enums;

public enum BetStatus // SON DE TIPO ENTERO PERO ENUMERABLE, SE USAN PARA REPRESENTAR EL ESTADO DE UNA APUESTA, YA SEA PENDIENTE, GANADA O PERDIDA
{
    Pending = 0,
    Won = 1,
    Lost = 2
}
