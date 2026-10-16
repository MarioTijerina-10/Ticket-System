using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TicketsApp.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly SignInManager<Usuario> signInManager;

        public AuthController(SignInManager<Usuario> _signInManager)
        {
            signInManager = _signInManager;
        }

        [HttpPost("login")]
        [IgnoreAntiforgeryToken]
        public async Task<ActionResult> Login([FromBody] LoginUsuario credenciales)
        {
            var respuesta = await signInManager.PasswordSignInAsync(userName: credenciales.Usuario, password:
            credenciales.Contraseña, isPersistent:
            false,
            lockoutOnFailure: false);   
            if (respuesta.Succeeded)
            {
                return Ok();
            }
            return Unauthorized();
        }

        [HttpPost("cerrarSesion")]
        public async Task<ActionResult> LogOut()
        {
            try
            {
                await signInManager.SignOutAsync();
                return Ok();
            }
            catch
            {
                return BadRequest("Login Invalido");
            }
        }
    }
}
