using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TicketsApp.DTO.Usuarios;
using System.Text.Json;
using MudBlazor;
using DocumentFormat.OpenXml.Drawing;

namespace TicketsApp.DTO.CamposEstaticos;

public class ServicioCamposEstaticos : ServicioBase<ServicioCamposEstaticos>
{
    private readonly UserManager<Usuario> userManager;
    private readonly IMemoryCache cache;
    private readonly RoleManager<IdentityRole> roleManager;

    public ServicioCamposEstaticos(UserManager<Usuario> _userManager, IMemoryCache _cache, RoleManager<IdentityRole> _roleManager, IDbContextFactory<TicketsDbContext> fabrica, ILogger<ServicioCamposEstaticos> Log) : base(fabrica, Log)
    {
        cache = _cache;
        roleManager = _roleManager;
        userManager = _userManager;
    }

    public async Task<List<Estatus>> GetEstatuses()
    {
        var Contexto = await fabrica.CreateDbContextAsync();
        return (await cache.GetOrCreateAsync("Estatuses", async entry =>
        {
            return await Contexto.Estatuses.ToListAsync();
        }))!;
    }

    public async Task<List<IdentityRole>> GetRoles()
    {
        var roles = await roleManager.Roles.ToListAsync();
        return roles;
    }

    public async Task<List<Unidad>> GetUnidades()
    {
        var Contexto = await fabrica.CreateDbContextAsync();
        return (await cache.GetOrCreateAsync("Unidades", async entry =>
        {
            return await Contexto.Unidades.Where(u => u.Activo == true).ToListAsync();
        }))!;
    }

    public async Task<List<Categoria>> GetCategorias()
    {
        var Contexto = await fabrica.CreateDbContextAsync();
        return (await cache.GetOrCreateAsync("Categorias", async entry =>
        {
            return await Contexto.Categorias.Where(c => c.Activo == true).ToListAsync();
        }))!;
    }

    public async Task<MensajeDeRespuesta<List<Usuario>>> GetAnalistas()
    {
        var respuesta = await EjecutarServicio(async () =>
        {
            var analistas = await userManager.GetUsersInRoleAsync("Analista");
            var admins = await userManager.GetUsersInRoleAsync("Admin");

            var Usuarios = analistas.Union(admins).Select(u => u.Id).ToList();
            var respuesta = await userManager.Users.
            Include(u => u.TicketFkAnalistaNavigations).
            Where(u => Usuarios.Contains(u.Id))
            .ToListAsync();

            return respuesta;
        },
        "Los Analistas Fueron Extraidos Correctamente",
        "Hubo Un Error Al Extraer Los Analistas");

        return respuesta;
    }

    public async Task<MensajeDeRespuesta<List<Usuario>>> GetAnalistasActivos(List<Usuario> analistas)
    {
        var respuesta = await EjecutarServicio(async () =>
        {
            var respuesta = analistas.Where(a => a.Activo == true).ToList();
            return respuesta;
        },
        "Los Analistas Activos Fueron Extraidos Correctamente",
        "Hubo Un Error Al Extraer Los Analistas Activos");

        return respuesta;
    }

    public async Task<MensajeDeRespuesta<List<Usuario>>> GetAnalistasConTickets(List<Usuario> analistas, DateRange rangoDeTiempo)
    {
        var respuesta = await EjecutarServicio(
            async () =>
            {
                analistas = analistas
                .Where(a => a.TicketFkAnalistaNavigations
                .Any(t => t.FechaInicio >= rangoDeTiempo.Start!.Value && t.FechaInicio <= rangoDeTiempo.End!.Value))
                .OrderByDescending(a => a.TicketFkAnalistaNavigations.Count())
                .ToList();

                return analistas;
            },
            "Analistas Filtrados Correctamente",
            "Hubo un Error al Filtrar Analistas"
        );

        return respuesta;
    }
}
