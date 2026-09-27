using GolBet.Repositories.Data;
using GolBet.Repositories.Implementations;
using GolBet.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


//Este es el nuevo código 
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Open generic registration: one line, a repository for every entity
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); // TYPEOF: QUIERE DECIR QUE CUALQUIER TIPO DE ENTIDAD QUE SE LE PASE, SE VA A RESOLVER CON LA IMPLEMENTACION GENERICA GenericRepository<T> PARA ESA ENTIDAD. ES DECIR, SI SE INYECTA IGenericRepository<Match>, SE VA A RESOLVER CON GenericRepository<Match>.

// Specific repositories
builder.Services.AddScoped<IMatchRepository, MatchRepository>(); // RECIBE UN MATCH REPOSITORY, QUE ES UNA IMPLEMENTACION ESPECIFICA DE LA INTERFAZ IMatchRepository, HEREDANDO DE LA CLASE GENERICA GenericRepository<Match>.



var app = builder.Build();


// Seed the database on startup
using (var scope = app.Services.CreateScope())   // SIRVE PARA LLAMAR AL SEEDER DE FORMA AUTOMATICA, PARA QUE CUANDO SE INICIE LA APLICACION, SE LLAME AL SEEDER Y SE LLENE LA BASE DE DATOS CON DATOS DE PRUEBA SI NO HAY DATOS. SI YA HAY DATOS, NO HACE NADA.
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbSeeder.SeedAsync(context);
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want
    // to change this for production scenarios, see
    // https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
