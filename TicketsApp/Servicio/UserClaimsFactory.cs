using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

public class UsuarioClaimsFactory : UserClaimsPrincipalFactory<Usuario, IdentityRole>
{
    public UsuarioClaimsFactory(
        UserManager<Usuario> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<IdentityOptions> options)
        : base(userManager, roleManager, options) { }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(Usuario user)
    {
        var identity = await base.GenerateClaimsAsync(user);
        identity.AddClaim(new Claim("Nombre", user.Nombre ?? ""));
        identity.AddClaim(new Claim("Id", user.Id ?? ""));
        identity.AddClaim(new Claim("Unidad", user.FkUnidad.ToString()));
        identity.AddClaim(new Claim("Departamento", user.FkDepartamento.ToString()));
        identity.AddClaim(new Claim("Correo", user.Email ?? ""));
        return identity;
    }
}