using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TicketsApp.DTO.Usuarios;
using TicketsApp.Helper_Classes;
using TicketsApp.Interfaces;

namespace TicketsApp.Services;

//Servicio para traer datos de usuarios de la base de datos.
//Utiliza la clase base de ServicioBase
public class ServicioUsuarios : ServicioBase
{
    private readonly UserManager<Usuario> userManager;
    private readonly RoleManager<IdentityRole> rolManager;
    public ServicioUsuarios(IDbContextFactory<TicketsDbContext> fabrica, UserManager<Usuario> _userManager, RoleManager<IdentityRole> _rolManager) : base(fabrica)
    {
        userManager = _userManager;
        rolManager = _rolManager;
    }

    public async Task<MensajeDeRespuesta<List<GetUsuario>>> GetAll()
    {
        var respuesta = await userManager.Users.Where(u => u.Activo == true).ToListAsync();
        List<GetUsuario> usuarios = new();

        foreach (var x in respuesta)
        {
            GetUsuario usuario = new();

            usuario.Contraseña = x.PasswordHash!;
            usuario.Correo = x.Email;
            usuario.Usuario = x.UserName!;
            usuario.Id = x.Id;
            usuario.Nombre = x.Nombre;
            usuario.Unidad = (Enumerables.Unidades)x.FkUnidad;
            usuario.Rol = await userManager.GetRolesAsync(x);

            usuarios.Add(usuario);
        }

        return new MensajeDeRespuesta<List<GetUsuario>>
        {
            exitoso = true,
            data = usuarios,
            mensaje = "Los usuarios fueron extraidos correctamente"
        };
    }

    public async Task<MensajeDeRespuesta<GetUsuario>> GetOne(string id)
    {
        var respuesta = await userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (respuesta == null)
        {
            return new MensajeDeRespuesta<GetUsuario>
            {
                exitoso = false,
                mensaje = "No se encontro ningun usuario."
            };
        }

        GetUsuario usuario = new();

        usuario.Contraseña = respuesta.PasswordHash!;
        usuario.Correo = respuesta.Email;
        usuario.Nombre = respuesta.Nombre;
        usuario.Unidad = (Enumerables.Unidades)respuesta.FkUnidad;
        usuario.Rol = await userManager.GetRolesAsync(respuesta);

        return new MensajeDeRespuesta<GetUsuario>
        {
            exitoso = true,
            data = usuario,
            mensaje = "El usuario fue extraido exitosamente."
        };
    }

    public async Task<MensajeDeRespuesta> CreateUsuario(CreateUsuario datos)
    {
        try
        {
            Usuario usuario = new();
            usuario.Email = datos.Correo;
            usuario.UserName = datos.Usuario1;
            usuario.FkUnidad = datos.FkUnidad;
            usuario.Nombre = datos.Nombre;

            Console.WriteLine(usuario);
            var resultado = await userManager.CreateAsync(usuario, datos.Contraseña);
            if (resultado.Succeeded)
                await userManager.AddToRoleAsync(usuario, datos.Rol);

            return new MensajeDeRespuesta
            {
                exitoso = true,
                mensaje = "El usaurio fue creado correctamente"
            };
        }
        catch (Exception ex)
        {
            return new MensajeDeRespuesta
            {
                exitoso = false,
                mensaje = "Hubo un fallo al cargar el usuario: " + ex
            };
        }
    }

    public async Task<MensajeDeRespuesta> UpdateUsuario(UpdateUsuario datos, string id)
    {
        var usuario = await userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (usuario == null)
        {
            return new MensajeDeRespuesta<GetUsuario>
            {
                exitoso = false,
                mensaje = "No se encontro ningun usuario."
            };
        }

        usuario.Email = datos.Correo;
        usuario.UserName = datos.Usuario1;
        usuario.FkUnidad = datos.FkUnidad;
        usuario.Nombre = datos.Nombre;
        await userManager.UpdateAsync(usuario);

        var rolActual = await userManager.GetRolesAsync(usuario);
        await userManager.RemoveFromRolesAsync(usuario, rolActual);
        await userManager.AddToRoleAsync(usuario, datos.Rol);
        await userManager.ChangePasswordAsync(usuario, usuario.PasswordHash!, datos.Contraseña);

        return new MensajeDeRespuesta
        {
            exitoso = true,
            mensaje = "El usuario fue actualzado correctamente"
        };
    }

    public async Task<MensajeDeRespuesta> DeleteUsuario(string id)
    {

        var usuario = await userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (usuario == null)
        {
            return new MensajeDeRespuesta<GetUsuario>
            {
                exitoso = false,
                mensaje = "No se encontro ningun usuario."
            };
        }

        usuario.Activo = false;
        await userManager.UpdateAsync(usuario);

        return new MensajeDeRespuesta
        {
            exitoso = true,
            mensaje = "El usuario fue eliminado correctamente"
        };
    }
}
