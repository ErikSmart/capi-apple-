using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Capi.Modelos;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


namespace Capi
{
    [EnableCors("PermitirApiRequest")]
    [Route("api/[Controller]")]
    [ApiController]
    public class CuentaController : ControllerBase
    {
        private DataContext context;
        private IMapper mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public CuentaController(
            UserManager<ApplicationUser> userManager,
            //Traer el RoleManager (importante)
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            DataContext context,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            this.context = context;
            this.mapper = mapper;
            this.roleManager = roleManager;

        }

        [HttpPost("Crear")]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] UserInfo model)
        {
            var user = new ApplicationUser { UserName = model.email, Email = model.email };
            var result = await _userManager.CreateAsync(user, model.contrasenia);
            if (result.Succeeded)
            {
                return BuildToken(model);
            }
            else
            {
                return BadRequest("Username or password invalid");
            }



        }
        //Creando el Role y asiganacion del role
        [HttpPost("mirole")]
        public async Task<ActionResult> mirole([FromBody] RoleInfo model)
        {
            var usurio = await _userManager.Users.SingleOrDefaultAsync(x => x.UserName == "e@g.com");
            var role = new IdentityRole { Name = model.name };
            //creando el Role
            var creado = await roleManager.CreateAsync(role);
            await _userManager.AddToRoleAsync(usurio, "Admin");

            return Ok(creado);
        }


        [HttpPost("Login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserInfo userInfo)
        {
            var result = await _signInManager.PasswordSignInAsync(userInfo.email, userInfo.contrasenia, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return BuildToken(userInfo);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return BadRequest(ModelState);
            }
        }
        [HttpGet("Login")]
        public async Task<ActionResult<IEnumerable<UsuariosDTO>>> Mostrar()
        {
            var result = await context.Users.ToListAsync();
            var pDTO = mapper.Map<List<UsuariosDTO>>(result);
            return pDTO;
        }

        private UserToken BuildToken(UserInfo userInfo)
        {
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.email),
        new Claim("miValor", "Lo que yo quiera"),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };
            //para que funcione "JWT:key" se tiene que colocar una en appsettings.json 
            /*  "JWT": {
    "key": "aKLMSLK3I4JNKNDKJFNKJN545N4J5N4J54H4G44H5JBSSDBNF3453S2223KJOP"
  } */
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Tiempo de expiración del token. En nuestro caso lo hacemos de una hora.
            var expiration = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}