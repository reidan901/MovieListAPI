using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieListAPI.DTO;
using MovieListAPI.Repositories;

namespace MovieListAPI.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<UserAsNormalDTO>> GetUsers()
        {
            return (await userRepository.GetUsersAsync()).Select(user => user.AsDto());
        }

        [HttpPost]
        public async Task<ActionResult> AddUser([FromForm]CreateUserNormalDTO user)
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

                await userRepository.InsertUserAsync(new Models.User
                {
                    Username = user.Username,
                    Password = user.Password,
                    role = Models.UserRole.NormalUser,
                    ImageName = user.Image.FileName,
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow
                });

                return StatusCode(201);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error.");
            }
        }
    }
}
