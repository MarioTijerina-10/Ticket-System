using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TicketsApp.DTO.Usuarios;

namespace TicketsApp.DTO.CamposEstaticos;

public class ServicioCamposEstaticos : ServicioBase
{
    private readonly IMemoryCache cache;
    private readonly RoleManager<IdentityRole> roleManager;

    public ServicioCamposEstaticos(IMemoryCache _cache, TicketsDbContext contexto, RoleManager<IdentityRole> _roleManager, IDbContextFactory<TicketsDbContext> fabrica) : base(fabrica)
    {
        cache = _cache;
        roleManager = _roleManager;
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
            return await Contexto.Unidades.ToListAsync();
        }))!;
    }

    public async Task<List<Categoria>> GetCategorias()
    {
        var Contexto = await fabrica.CreateDbContextAsync();
        return (await cache.GetOrCreateAsync("Categorias", async entry =>
        {
            return await Contexto.Categorias.ToListAsync();
        }))!;
    }
}
