using GolBet.Entities;
using GolBet.Repositories.Data;
using GolBet.Repositories.Implementations;
using GolBet.Repositories.Interfaces;
using GolBet.Services.Implementations;
using GolBet.Services.Interfaces;
using GolBet.Services.Mapping;
using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

// Spanish (Colombia) dates, but "." as decimal separator so odds parse the same in forms and views
var colombia = (CultureInfo)CultureInfo.GetCultureInfo("es-CO").Clone();
colombia.NumberFormat.NumberDecimalSeparator = ".";
CultureInfo.DefaultThreadCurrentCulture = colombia;
CultureInfo.DefaultThreadCurrentUICulture = colombia;

// Add services to the container.
builder.Services.AddControllersWithViews();


//Este es el nuevo código 
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ASP.NET Core Identity (Module 7): users, roles, passwords and cookies
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    // Academic-friendly password policy (production would be stricter)
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = true;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";            // anonymous hitting [Authorize]
    options.AccessDeniedPath = "/Account/AccessDenied"; // wrong role
});

// Open generic registration: one line, a repository for every entity
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); // TYPEOF: QUIERE DECIR QUE CUALQUIER TIPO DE ENTIDAD QUE SE LE PASE, SE VA A RESOLVER CON LA IMPLEMENTACION GENERICA GenericRepository<T> PARA ESA ENTIDAD. ES DECIR, SI SE INYECTA IGenericRepository<Match>, SE VA A RESOLVER CON GenericRepository<Match>.

// Specific repositories
builder.Services.AddScoped<IMatchRepository, MatchRepository>(); // RECIBE UN MATCH REPOSITORY, QUE ES UNA IMPLEMENTACION ESPECIFICA DE LA INTERFAZ IMatchRepository, HEREDANDO DE LA CLASE GENERICA GenericRepository<Match>.

// AutoMapper: scans the assembly containing MappingProfile for all profiles
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Business services
builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<ITeamService, TeamService>();


var app = builder.Build();


// Seed the database on startup
using (var scope = app.Services.CreateScope())   // SIRVE PARA LLAMAR AL SEEDER DE FORMA AUTOMATICA, PARA QUE CUANDO SE INICIE LA APLICACION, SE LLAME AL SEEDER Y SE LLENE LA BASE DE DATOS CON DATOS DE PRUEBA SI NO HAY DATOS. SI YA HAY DATOS, NO HACE NADA.
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    await DbSeeder.SeedAsync(context, roleManager, userManager);
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

// Order matters: who are you? (cookie) BEFORE may you do this? (authorization)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
