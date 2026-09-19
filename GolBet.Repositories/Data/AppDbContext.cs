// GolBet.Repositories/Data/AppDbContext.cs
using GolBet.Entities;
using GolBet.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace GolBet.Repositories.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    // CADA TABLA SE LE ASIGNA UN DbSet<T> QUE REPRESENTA LA TABLA EN LA BASE DE DATOS, DONDE T ES EL TIPO DE ENTIDAD QUE REPRESENTA LA TABLA
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<Match> Matches => Set<Match>();
    public DbSet<Bet> Bets => Set<Bet>();
    // SI MAÑANA HAGO UNA TABLA NUEVA, SOLO TENGO QUE AGREGAR UN NUEVO DbSet<T> AQUI Y YA ESTA, NO HAY QUE HACER NADA MAS
    // EL NOMBRE DE LAS TABLAS DEBE SER EN PLURAL, PORQUE CADA TABLA REPRESENTA UNA COLECCION DE ENTIDADES, NO UNA ENTIDAD INDIVIDUAL

    protected override void OnModelCreating(ModelBuilder modelBuilder)// LO CREA EL ENTITY FRAMEWORK, PERO SE PUEDE SOBREESCRIBIR PARA CONFIGURAR LA BASE DE DATOS, COMO POR EJEMPLO LAS RELACIONES ENTRE TABLAS, LOS INDICES, LOS TIPOS DE DATOS, ETC.
        
    {
        base.OnModelCreating(modelBuilder);

        // Team names must be unique (case- and accent-insensitive)
        modelBuilder.Entity<Team>()
            .Property(t => t.Name)
            .UseCollation("SQL_Latin1_General_CP1_CI_AI"); //Esta parte me indica el Sensitive Case


        modelBuilder.Entity<Team>() // VOY A LA TABLA
            .HasIndex(t => t.Name)// VOY A LA PROPIEDAD NAME DE LA TABLA TEAM
            .IsUnique();// LE DIGO QUE EL INDICE DEBE SER UNICO, PARA QUE NO HAYA DOS EQUIPOS CON EL MISMO NOMBRE

        // Double relationship Match -> Team: convention cannot resolve it
        modelBuilder.Entity<Match>()
            .HasOne(m => m.HomeTeam)
            .WithMany()
            .HasForeignKey(m => m.HomeTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Match>()
            .HasOne(m => m.AwayTeam)
            .WithMany()
            .HasForeignKey(m => m.AwayTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        // A match with bets cannot be deleted
        modelBuilder.Entity<Bet>()
            .HasOne(b => b.Match)  // UN PARTIDO 
            .WithMany(m => m.Bets) // TIENE MUCHAS APUESTAS
            .OnDelete(DeleteBehavior.Restrict);
    }

    // ---- Automatic audit timestamps ----
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var utcNow = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = utcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedDate = utcNow;
                    // CreatedDate must never change after creation
                    entry.Property(e => e.CreatedDate).IsModified = false;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
