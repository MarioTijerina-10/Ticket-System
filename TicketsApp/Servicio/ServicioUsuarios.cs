using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TicketsApp.DTO;
using TicketsApp.DTO.Usuarios;

namespace TicketsApp.Services;

// Servicio para traer datos de usuarios de la base de datos.
// Utiliza la clase base de ServicioBase
public class ServicioUsuarios : ServicioBase<ServicioUsuarios>
{
    private readonly UserManager<Usuario> userManager;
    private readonly RoleManager<IdentityRole> rolManager;

    public ServicioUsuarios(
        IDbContextFactory<TicketsDbContext> fabrica,
        UserManager<Usuario> userManager,
        RoleManager<IdentityRole> rolManager,
        ILogger<ServicioUsuarios> log)
        : base(fabrica, log)
    {
        this.userManager = userManager;
        this.rolManager = rolManager;
    }

    public async Task<MensajeDeRespuesta<List<GetUsuario>>> GetAll()
    {
        return await EjecutarServicio(
            async () =>
            {
                var respuesta = await userManager.Users
                    .Include(u => u.FkDepartamentoNavigation)
                    .Include(u => u.FkUnidadNavigation)
                    .Where(u => u.Activo)
                    .AsNoTracking()
                    .ToListAsync();

                List<GetUsuario> usuarios = new();

                foreach (var x in respuesta)
                {
                    usuarios.Add(new GetUsuario
                    {
                        Id = x.Id,
                        Nombre = x.Nombre,
                        Usuario = x.UserName!,
                        Correo = x.Email,
                        Contraseña = x.PasswordHash!,
                        ContraseñaVisible = x.ContraseñaVisible,
                        Departamento = x.FkDepartamentoNavigation.Nombre,
                        FkDepartamento = x.FkDepartamento,
                        Unidad = x.FkUnidadNavigation.Nombre,
                        FkUnidad = x.FkUnidad,
                        Rol = await userManager.GetRolesAsync(x)
                    });
                }

                return usuarios;
            },
            mensajeDeExito: "Los usuarios fueron extraídos correctamente",
            mensajeDeError: "Hubo un error al extraer los usuarios");
    }

    public async Task<MensajeDeRespuesta<GetUsuario>> GetOne(string id)
    {
        return await EjecutarServicio(
            async () =>
            {
                var respuesta = await userManager.Users
                    .Include(u => u.FkDepartamentoNavigation)
                    .Include(u => u.FkUnidadNavigation)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (respuesta is null)
                    throw new InvalidOperationException("No se encontró ningún usuario");

                return new GetUsuario
                {
                    Id = respuesta.Id,
                    Nombre = respuesta.Nombre,
                    Usuario = respuesta.UserName!,
                    Correo = respuesta.Email,
                    Contraseña = respuesta.PasswordHash!,
                    ContraseñaVisible = respuesta.ContraseñaVisible,
                    Departamento = respuesta.FkDepartamentoNavigation.Nombre,
                    FkDepartamento = respuesta.FkDepartamento,
                    Unidad = respuesta.FkUnidadNavigation.Nombre,
                    FkUnidad = respuesta.FkUnidad,
                    Rol = await userManager.GetRolesAsync(respuesta)
                };
            },
            mensajeDeExito: "El usuario fue extraído exitosamente",
            mensajeDeError: "No se encontró ningún usuario");
    }

    public async Task<MensajeDeRespuesta<string>> CrearUsuario(CreateUsuario datos)
    {
        return await EjecutarServicio(
            async () =>
            {
                var usuario = new Usuario
                {
                    Email = datos.Correo,
                    ContraseñaVisible = datos.Contraseña,
                    UserName = datos.Usuario1,
                    FkUnidad = datos.FkUnidad,
                    FkDepartamento = datos.FkDepartamento,
                    Nombre = datos.Nombre
                };

                var resultado = await userManager.CreateAsync(usuario, datos.Contraseña);

                if (!resultado.Succeeded)
                {
                    var errores = string.Join("; ", resultado.Errors.Select(e => e.Description));
                    throw new InvalidOperationException(errores);
                }

                await userManager.AddToRoleAsync(usuario, datos.Rol);

                return usuario.Id;
            },
            mensajeDeExito: "El usuario fue creado correctamente",
            mensajeDeError: "Hubo un fallo al crear el usuario");
    }

    public async Task<MensajeDeRespuesta<bool>> UpdateUsuario(UpdateUsuario datos, string id)
    {
        return await EjecutarServicio(
            async () =>
            {
                var usuario = await userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (usuario is null)
                    throw new InvalidOperationException("No se encontró ningún usuario");

                usuario.Email = datos.Correo;
                usuario.ContraseñaVisible = datos.Contraseña;
                usuario.UserName = datos.Usuario1;
                usuario.FkUnidad = datos.FkUnidad;
                usuario.FkDepartamento = datos.FkDepartamento;
                usuario.Nombre = datos.Nombre;

                var updateResult = await userManager.UpdateAsync(usuario);
                if (!updateResult.Succeeded)
                    throw new InvalidOperationException(string.Join("; ", updateResult.Errors.Select(e => e.Description)));

                var rolesActuales = await userManager.GetRolesAsync(usuario);
                await userManager.RemoveFromRolesAsync(usuario, rolesActuales);
                await userManager.AddToRoleAsync(usuario, datos.Rol);

                if (!string.IsNullOrEmpty(datos.Contraseña))
                {
                    await userManager.RemovePasswordAsync(usuario);
                    var passResult = await userManager.AddPasswordAsync(usuario, datos.Contraseña);
                    if (!passResult.Succeeded)
                        throw new InvalidOperationException(string.Join("; ", passResult.Errors.Select(e => e.Description)));
                }

                return true;
            },
            mensajeDeExito: "El usuario fue editado correctamente",
            mensajeDeError: "Hubo un error al intentar actualizar el usuario");
    }

    public async Task<MensajeDeRespuesta<bool>> DeleteUsuario(string id)
    {
        return await EjecutarServicio(
            async () =>
            {
                var usuario = await userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (usuario is null)
                    throw new InvalidOperationException("No se encontró ningún usuario");

                usuario.Activo = false;
                await userManager.UpdateAsync(usuario);

                return true;
            },
            mensajeDeExito: "El usuario fue eliminado correctamente",
            mensajeDeError: "Hubo un error al intentar eliminar el usuario");
    }

    public async Task<MensajeDeRespuesta<List<GetUsuario>>> FiltrarUsuarios(
        FiltroDeUsuarios filtrado, List<GetUsuario> usuarios)
    {
        return await EjecutarServicio(
            () =>
            {
                IEnumerable<GetUsuario> query = usuarios;

                if (!string.IsNullOrEmpty(filtrado.Nombre))
                    query = query.Where(u => u.Nombre.Contains(filtrado.Nombre, StringComparison.OrdinalIgnoreCase));

                if (!string.IsNullOrEmpty(filtrado.Rol))
                    query = query.Where(u => u.Rol.Contains(filtrado.Rol));

                if (filtrado.FkDepartamento != null)
                    query = query.Where(u => u.FkDepartamento == filtrado.FkDepartamento);

                if (filtrado.FkUnidad != null)
                    query = query.Where(u => u.FkUnidad == filtrado.FkUnidad);

                return Task.FromResult(query.ToList());
            },
            mensajeDeExito: "Datos extraídos correctamente",
            mensajeDeError: "Hubo un error al intentar filtrar los usuarios");
    }
}