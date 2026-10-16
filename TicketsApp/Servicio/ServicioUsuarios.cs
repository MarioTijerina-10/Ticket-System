using Microsoft.AspNetCore.Identity;
using TicketsApp.DTO.Usuarios;

namespace TicketsApp.Services;

// Servicio para traer datos de usuarios de la base de datos.
// Utiliza la clase base de ServicioBase
public class ServicioUsuarios : ServicioBase<ServicioUsuarios>
{
    private readonly UserManager<Usuario> userManager;

    public ServicioUsuarios(
        IDbContextFactory<TicketsDbContext> fabrica,
        UserManager<Usuario> userManager,
        ILogger<ServicioUsuarios> log)
        : base(fabrica, log)
    {
        this.userManager = userManager;
    }

    public async Task<MensajeDeRespuesta<List<GetUsuario>>> GetUserName()
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
                        ContraseñaVisible = x.ContraseñaVisible ?? "",
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
                        ContraseñaVisible = x.ContraseñaVisible ?? "",
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
                    ContraseñaVisible = respuesta.ContraseñaVisible ?? "",
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
                    string mensaje = "";

                    if (resultado.Errors.Any(e => e.Code == "DuplicateUserName"))
                        mensaje = "Ese usuario ya esta o ha estado en uso";
                    else if (resultado.Errors.Any(e => e.Code.StartsWith("Password")))
                        mensaje = "La contraseña no cumple con los parametros.";

                    throw new InvalidOperationException(mensaje);
                }

                await userManager.AddToRoleAsync(usuario, datos.Rol);
                return usuario.Id;
            },
            mensajeDeExito: "El usuario fue creado correctamente",
            mensajeDeError: "Hubo un fallo al crear el usuario", mensajeDelegadoError: mensaje => $"Hubo un fallo al crear el usuario: {mensaje}");
    }
    public async Task<MensajeDeRespuesta<bool>> UpdateUsuario(UpdateUsuario datos, string id)
    {
        return await EjecutarServicio(
            async () =>
            {
                string mensajeDeError = "";
                var usuario = await userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (usuario is null)
                    throw new InvalidOperationException("No se encontró ningún usuario");

                usuario.Email = datos.Correo;
                usuario.UserName = datos.Usuario1;
                usuario.FkUnidad = datos.FkUnidad;
                usuario.FkDepartamento = datos.FkDepartamento;
                usuario.Nombre = datos.Nombre;

                // Mantener sincronizados los valores normalizados
                await userManager.UpdateNormalizedUserNameAsync(usuario);
                await userManager.UpdateNormalizedEmailAsync(usuario);

                // 1) Datos del usuario (valida usuario y correo duplicados)
                var updateResult = await userManager.UpdateAsync(usuario);
                if (!updateResult.Succeeded)
                {
                    mensajeDeError = ObtenerMensajeError(updateResult);
                    throw new InvalidOperationException(ObtenerMensajeError(updateResult));
                }

                // 2) Roles
                var rolesActuales = await userManager.GetRolesAsync(usuario);
                if (!rolesActuales.Contains(datos.Rol) || rolesActuales.Count > 1)
                {
                    var quitar = await userManager.RemoveFromRolesAsync(usuario, rolesActuales);
                    if (!quitar.Succeeded)
                    {
                        throw new InvalidOperationException(ObtenerMensajeError(quitar));
                        mensajeDeError = ObtenerMensajeError(quitar);
                    }

                    var agregar = await userManager.AddToRoleAsync(usuario, datos.Rol);
                    if (!agregar.Succeeded)
                    {
                        mensajeDeError = ObtenerMensajeError(agregar);
                        throw new InvalidOperationException(ObtenerMensajeError(agregar));

                    }
                }

                // 3) Contraseña (solo si se envió una nueva)
                if (!string.IsNullOrEmpty(datos.Contraseña))
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(usuario);
                    var passResult = await userManager.ResetPasswordAsync(usuario, token, datos.Contraseña);
                    if (!passResult.Succeeded)
                    {
                        mensajeDeError = ObtenerMensajeError(passResult);
                        throw new InvalidOperationException(ObtenerMensajeError(passResult));
                    }


                    usuario.ContraseñaVisible = datos.Contraseña;
                    await userManager.UpdateAsync(usuario);
                }

                return true;
            },
            mensajeDeExito: "El usuario fue editado correctamente",
            mensajeDeError: "Hubo un error al intentar actualizar el usuario", mensajeDelegadoError: mensajeDeError => $"Hubo un error al intentar actualizar el usuario: {mensajeDeError}");
    }

    private static string ObtenerMensajeError(IdentityResult resultado)
    {
        var mensajes = resultado.Errors.Select(e => e.Code switch
        {
            "DuplicateUserName" => "Ese nombre de usuario ya está en uso.",
            "InvalidUserName" => "El nombre de usuario contiene caracteres no permitidos.",
            "DuplicateEmail" => "Ese correo ya está registrado.",
            "InvalidEmail" => "El formato del correo no es válido.",
            "ConcurrencyFailure" => "El usuario fue modificado por otra persona. Recarga e intenta de nuevo.",
            "PasswordTooShort" => "La contraseña es demasiado corta.",
            "PasswordRequiresDigit" => "La contraseña debe incluir al menos un número.",
            "PasswordRequiresLower" => "La contraseña debe incluir una minúscula.",
            "PasswordRequiresUpper" => "La contraseña debe incluir una mayúscula.",
            "PasswordRequiresNonAlphanumeric" => "La contraseña debe incluir un símbolo.",
            "PasswordRequiresUniqueChars" => "La contraseña necesita más caracteres distintos.",
            "UserAlreadyInRole" => "El usuario ya tiene ese rol.",
            "InvalidRoleName" => "El rol indicado no es válido.",
            _ => e.Description
        });

        return string.Join(" ", mensajes);
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
                await userManager.SetLockoutEnabledAsync(usuario, true);
                await userManager.SetLockoutEndDateAsync(usuario, null);
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