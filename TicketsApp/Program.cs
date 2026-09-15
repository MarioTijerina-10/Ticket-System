using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using MudBlazor.Services;
using MudBlazor.Translations;
using TicketsApp.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TicketsApp.Servicio;
using Microsoft.Extensions.Options;
using System.Reflection.Emit;
using TicketsApp.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Components;
using TicketsApp.Servicios;
using DocumentFormat.OpenXml.VariantTypes;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

async Task SeedRoles(IServiceProvider services)
{
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = { "Admin", "Analista", "Usuario" };

    foreach (var rol in roles)
    {
        if (!await roleManager.RoleExistsAsync(rol))
        {
            await roleManager.CreateAsync(new IdentityRole(rol));
        }
    }
}

// Add MudBlazor services
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.SnackbarVariant = MudBlazor.Variant.Outlined;
    config.SnackbarConfiguration.VisibleStateDuration = 3000;
});
builder.Services.AddMudTranslations();
builder.Services.AddDbContextFactory<TicketsDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDefaultIdentity<Usuario>(opc =>
{
    opc.SignIn.RequireConfirmedAccount = false;
}).AddRoles<IdentityRole>().AddEntityFrameworkStores<TicketsDbContext>().AddSignInManager();

builder.Services.Configure<IdentityOptions>(opt =>
{
    opt.Password.RequireDigit = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequiredLength = 1;
});
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddControllers();
builder.Services.AddAuthorization();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/InicioDeSesion";
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped<ServicioUsuarios>();
builder.Services.AddScoped<ServicioTickets>();
builder.Services.AddScoped<ServicioCamposEstaticos>();
builder.Services.AddScoped<ServicioComentarios>();
builder.Services.AddScoped<ServicioProveedores>();
builder.Services.AddScoped<ServicioUnidades>();
builder.Services.AddScoped<ServicioCategorias>();
builder.Services.AddScoped<ServicioDepartamentos>();
builder.Services.AddScoped<PlantillasCorreo>();
builder.Services.AddTransient<ServicioCorreo>();
builder.Services.AddSingleton<EmailQueue>();
builder.Services.AddHostedService<ServicoCerrarTickets>();
builder.Services.AddHostedService<ServicioDeFondoCorreo>();

builder.Services.AddScoped(sp =>
{
    var navigation = sp.GetRequiredService<NavigationManager>();
    return new HttpClient
    {
        BaseAddress = new Uri(navigation.BaseUri)
    };
});
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateAsyncScope())
{
    var services = scope.ServiceProvider;
    await SeedRoles(scope.ServiceProvider);
}

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapRazorPages();
app.MapControllers();

app.Run();
