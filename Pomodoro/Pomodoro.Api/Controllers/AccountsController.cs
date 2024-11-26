using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pomodoro.API.DATA;
using Pomodoro.Shared.Entities;
using Pomodoro.Shared.Dtos;
using Microsoft.IdentityModel.Tokens;
using Pomodoro.API.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

//
namespace Pomodoro.API.Controllers
{
    [ApiController]

    [Route("/api/accounts")]// Ruta para acceder a este controlador.

    public class AccountsController : ControllerBase

    {
        // Dependencias del controlador: ayudan a manejar los usuarios y configuraciones.

        private readonly IUserHelper _userHelper;// Helper para operaciones relacionadas con usuarios.

        private readonly IConfiguration _configuration;// Permite acceder a configuraciones como claves de seguridad.

        private readonly IFileStorage _fileStorage;

        private readonly string _container;


        //Constructor
        public AccountsController(IUserHelper userHelper, IConfiguration configuration, IFileStorage fileStorage)

        {

            _userHelper = userHelper;

            _configuration = configuration;

            _fileStorage = fileStorage;

            _container = "users";

        }


        //crea un nuevo usuario
        [HttpPost("CreateUser")]

        public async Task<ActionResult> CreateUser([FromBody] UserDTO model)

        {

            User user = model;//convierte el DTO en una entidad User

            var result = await _userHelper.AddUserAsync(user, model.Password);

            if (!string.IsNullOrEmpty(model.Photo))

            {

                var photoUser = Convert.FromBase64String(model.Photo);

                model.Photo = await _fileStorage.SaveFileAsync(photoUser, ".jpg", _container);

            }

            if (result.Succeeded)//si la creacion sale bien

            {
                //asigna un rol al usuario basado en su tipo
                await _userHelper.AddUserToRoleAsync(user, user.UserType.ToString());
                //devuelve el token de autenticacion
                return Ok(BuildToken(user));

            }


            //devuelve el error
            return BadRequest(result.Errors.FirstOrDefault());

        }




        //autenticacion
        [HttpPost("Login")]

        public async Task<ActionResult> Login([FromBody] LoginDTO model)

        {
            //verefica las credencialesdel usuario
            var result = await _userHelper.LoginAsync(model);

            if (result.Succeeded)

            {
                //obtiene los datos del usuario para generar el token
                var user = await _userHelper.GetUserAsync(model.Email);

                return Ok(BuildToken(user));

            }



            return BadRequest("Email o contraseña incorrectos.");

        }


        //genera un token JWT para un usuario autenticado
        private TokenDTO BuildToken(User user)

        {

            var claims = new List<Claim>

            {
                //reclama cada dato del usuario
                new Claim(ClaimTypes.Name, user.Email!),

                new Claim(ClaimTypes.Role, user.UserType.ToString()),

                new Claim("Document", user.Document),

                new Claim("FirstName", user.FirstName),

                new Claim("LastName", user.LastName),

                new Claim("Photo", user.Photo ?? string.Empty)

            };


            //obtiene la clave de ka firma desde la configuracion
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwtKey"]!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);//algoritmo de firma

            var expiration = DateTime.UtcNow.AddDays(30); //establece la expiracion del token de 30 dias

            //crea el token JWT
            var token = new JwtSecurityToken(

                issuer: null,//emisor

                audience: null,//audiencia

                claims: claims,//datos del usuario en el token

                expires: expiration,//expiracion

                signingCredentials: credentials);//credenciales de firma



            return new TokenDTO//devuelve el token con su fecha de expiracion

            {

                Token = new JwtSecurityTokenHandler().WriteToken(token),//convierte eltoken a texto

                Expiration = expiration

            };

        }
        [HttpPut]

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<ActionResult> Put(User user)

        {

            try

            {

                if (!string.IsNullOrEmpty(user.Photo))

                {

                    var photoUser = Convert.FromBase64String(user.Photo);

                    user.Photo = await _fileStorage.SaveFileAsync(photoUser, ".jpg", _container);

                }



                var currentUser = await _userHelper.GetUserAsync(user.Email!);

                if (currentUser == null)

                {

                    return NotFound();

                }



                currentUser.Document = user.Document;

                currentUser.FirstName = user.FirstName;

                currentUser.LastName = user.LastName;

                currentUser.PhoneNumber = user.PhoneNumber;

                currentUser.Photo = !string.IsNullOrEmpty(user.Photo) && user.Photo != currentUser.Photo ? user.Photo : currentUser.Photo;



                var result = await _userHelper.UpdateUserAsync(currentUser);

                if (result.Succeeded)

                {

                    return NoContent();

                }



                return BadRequest(result.Errors.FirstOrDefault());

            }

            catch (Exception ex)

            {

                return BadRequest(ex.Message);

            }

        }



        [HttpGet]

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<ActionResult> Get()

        {

            return Ok(await _userHelper.GetUserAsync(User.Identity!.Name!));

        }

        [HttpPost("changePassword")]

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<ActionResult> ChangePasswordAsync(ChangePasswordDTO model)

        {

            if (!ModelState.IsValid)

            {

                return BadRequest(ModelState);

            }



            var user = await _userHelper.GetUserAsync(User.Identity!.Name!);

            if (user == null)

            {

                return NotFound();

            }



            var result = await _userHelper.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (!result.Succeeded)

            {

                return BadRequest(result.Errors.FirstOrDefault().Description);

            }



            return NoContent();

        }

    }
}
