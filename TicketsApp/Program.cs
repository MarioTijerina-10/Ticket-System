using MudBlazor.Services;
using MudBlazor.Translations;
using TicketsApp.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components;
using TicketsApp.Servicios;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.HttpOverrides;
using MudBlazor;

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

//DEPENDIENDO DEL ENTORNO DE TRABAJO SE CONFIGURA EL LAS VARIABLES DE ENTRONO PARA LOS DIFERENTES SERVICIOS COMO
//BASE DE DATOS, SERVICIO DE CORREO.

string? cs = "";

if (builder.Environment.IsDevelopment())
{
    cs = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
}

else if (builder.Environment.IsStaging() || builder.Environment.IsProduction())
{
    var csDeployment = new SqlConnectionStringBuilder()
    {
        DataSource = Environment.GetEnvironmentVariable("DB_SERVER"),
        InitialCatalog = Environment.GetEnvironmentVariable("DB_NAME"),
        UserID = Environment.GetEnvironmentVariable("DB_USER"),
        Password = Environment.GetEnvironmentVariable("DB_PASSWORD"),
        TrustServerCertificate = true
    };

    cs = csDeployment.ConnectionString;
}

builder.Services.AddDbContextFactory<TicketsDbContext>(options => options.UseSqlServer(cs));

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
builder.Services.AddScoped<ServicioPlantillas>();
builder.Services.AddScoped<PlantillasCorreo>();
builder.Services.AddScoped<IUserClaimsPrincipalFactory<Usuario>, UsuarioClaimsFactory>();
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

app.UseAuthentication();
app.UseAuthorization();

//REDIRIGE DE LA BASE RUTA AL LOGIN DE USUARIO
app.MapGet("/", () => Results.Redirect("/InicioDeSesion"));

using (var scope = app.Services.CreateAsyncScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<TicketsDbContext>();

        //MIGRACION AUTOMATICA AL INICIAR O DESPLEGUAR APLICACION
        await db.Database.MigrateAsync();

        //CREACION DE ROLES EN LA BASE DE DATOS
        var services = scope.ServiceProvider;
        await SeedRoles(scope.ServiceProvider);

        //ASIGNACION DE USUARIO Y CONTRASEÑA PARA USUARIO ADMIN, DEPENDIENDO DEL ENTORNO DEL 
        //PROYECTO SE ASIGNARAN LAS VARIABLES DESDE EL ENTORNDE VARIABLES DE DOCKPLOY EN UN DESPLIEGUE A PRODUCCION 
        //O PRUEBAS. PERO LOCALMENTE SE UTILIZARAN SECRETOS ASIGNADOS POR EL USUARIO DENTRO DE "dotnet user-secrets" DESDE LA TERMINAL DEL PROYECTO
        // dotnet user-secrets init
        // dotnet user-secrets set "AdminUser:ADMIN_USER" "Usuario"
        // dotnet user-secrets set "AdminUser:ADMIN_PASSWORD" "Password"

        string? adminUser = null;
        string? adminContraseña = null;

        if (app.Environment.IsDevelopment())
        {
            adminUser = builder.Configuration["AdminUser:ADMIN_USER"];
            adminContraseña = builder.Configuration["AdminUser:ADMIN_PASSWORD"];
            Console.WriteLine("Desarrollo");
        }

        else if (app.Environment.IsStaging() || app.Environment.IsProduction())
        {
            adminUser = Environment.GetEnvironmentVariable("ADMIN_USER");
            adminContraseña = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");
            Console.WriteLine("Produccion");
        }

        if (adminContraseña != null && adminUser != null)
        {
            //PRIMERO SE REVISA QUE EL USUARIO ADMIN NO ESTA CREADO YA
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Usuario>>();
            var existeUsuarioAdmin = await userManager.FindByNameAsync(adminUser);

            if (existeUsuarioAdmin == null)
            {
                //RETRIVE LOS FEPARTAMENTO Y UNIDAD PARA EL USUARIO DEFECTO
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

                var respuesta = await userManager.CreateAsync(admin, adminContraseña);
                if (respuesta.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                    Console.WriteLine($"Usuario admin creado correctamente");
                }

                else
                {
                    foreach (var x in respuesta.Errors)
                    {
                        Console.WriteLine($"Error al crear usuario admin: {x.Description}");
                    }
                }
            }
        }

        else
        {
            Console.WriteLine("No se encontro contraseña o usuario para usuario admin");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Usuarios o contraseña de SuperAdmin Tuvo un error al intentar crearse, Psoible falta de generacion de Credenciales: {ex.Message}, {ex.InnerException}");
    }
}

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapRazorPages();
app.MapControllers();

app.Run();
