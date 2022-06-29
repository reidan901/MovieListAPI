using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovieListAPI.DTO;
using MovieListAPI.Models;
using MovieListAPI.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieListAPI.Controllers
{
    [ApiController]
    [Route("users")]
    public class LoginController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IConfiguration _config;

        public LoginController(IUnitOfWork uOF,IConfiguration config)
        {
            unitOfWork = uOF;
            _config = config;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<ActionResult> Register([FromForm] CreateUserNormalDTO user)
        {
            try
            {
                if (user == null)
                    return BadRequest();
                string workingDirectory = Environment.CurrentDirectory + "\\Upload\\" + user.Image.FileName;
                using (Stream fileStream = new FileStream(workingDirectory, FileMode.Create))
                {
                    await user.Image.CopyToAsync(fileStream);
                }

                await unitOfWork.UserRepository.InsertUserAsync(new Models.User
                {
                    Username = user.Username,
                    Password = user.Password,
                    role = Models.UserRole.NormalUser,
                    ImageName = user.Image.FileName,
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow
                });

                return Ok(await unitOfWork.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error.");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<ActionResult> AddUser([FromForm] LoginUserDTO user)
        {
            try
            {
                var foundUser= await unitOfWork.UserRepository.GetUserByUserNameAsync(user.Username);
                if (foundUser == null)
                    return NotFound("No user exists with that username.");
                return Ok(GenerateToken(foundUser));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error.");
            }
        }

        [NonAction]
        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Role,user.role.ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddMinutes(15),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
