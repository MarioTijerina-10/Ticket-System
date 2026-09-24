using MudBlazor.Services;
using MudBlazor.Translations;
using TicketsApp.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components;
using TicketsApp.Servicios;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.HttpOverrides;

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

var cs = new SqlConnectionStringBuilder()
{
    DataSource = Environment.GetEnvironmentVariable("DB_SERVER"),
    InitialCatalog = Environment.GetEnvironmentVariable("DB_NAME"),
    UserID = Environment.GetEnvironmentVariable("DB_USER"),
    Password = Environment.GetEnvironmentVariable("DB_PASSWORD"),
    TrustServerCertificate = true
};

builder.Services.AddDbContextFactory<TicketsDbContext>(options => options.UseSqlServer(cs.ConnectionString));

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

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateAsyncScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<TicketsDbContext>();
        await db.Database.MigrateAsync();

        // añades los roles al iniciar la app
        var services = scope.ServiceProvider;
        await SeedRoles(scope.ServiceProvider);

        //Corre migraciones a la base de datos al correr la aplicacion.
        var adminUser = Environment.GetEnvironmentVariable("ADMIN_USER");
        var adminContraseña = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Usuario>>();

        if (adminContraseña != null && adminUser != null)
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var servicioDepartamentos = scope.ServiceProvider.GetRequiredService<ServicioDepartamentos>();
            var servicioUnidades = scope.ServiceProvider.GetRequiredService<ServicioUnidades>();
            var adminUserFkDepartamento = (await servicioDepartamentos.GetPrimerFk()).data;
            var adminUserFkUnidad = (await servicioUnidades.GetPrimerFk()).data;

            var admin = new Usuario()
            {
                Nombre = "Administrador",
                UserName = adminUser,
                ContraseñaVisible = adminContraseña,
                FkDepartamento = adminUserFkDepartamento,
                FkUnidad = adminUserFkUnidad,
            };

            await userManager.CreateAsync(admin, adminContraseña);
            await userManager.AddToRoleAsync(admin, "Admin");
        }

        Console.WriteLine("Usuarios SuperAdmin Fue creado correctamente");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Usuarios SuperAdmin Tuvo un error al intentar crearse: {ex.Message}, {ex.InnerException}");
    }
}

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapRazorPages();
app.MapControllers();

app.Run();
