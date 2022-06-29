using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieListAPI.DTO;
using MovieListAPI.Models;
using MovieListAPI.Repositories;

namespace MovieListAPI.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration _config;

        public UserController(IUnitOfWork uOF, IConfiguration config)
        {
            unitOfWork = uOF;
            _config = config;
        }

        [HttpGet]
        [Authorize(Roles = "NormalUser,Admin")]
        public async Task<IEnumerable<UserNormalAcessDTO>> GetUsersNormalEndpoint()
        {
            return (await unitOfWork.UserRepository.GetUsersAsync()).Select(user => user.AsDtoNormal());
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IEnumerable<UserAdminAcessDTO>> GetUsersAdminEndpoint()
        {
            return (await unitOfWork.UserRepository.GetUsersAsync()).Select(user => user.AsDtoAdmin());
        }

        [HttpPatch]
        [Authorize(Roles = "NormalUser,Admin")]
        public async Task<ActionResult> UpdateUserNormalEndpoint([FromForm] UpdateUserNormalDTO newUser)
        {
            try
            {
                var existingUser = await unitOfWork.UserRepository.GetUserByIDAsync(newUser.UserId);
                if (existingUser == null)
                    return NotFound("User doesn't exist in the database.");
                string workingDirectory = Environment.CurrentDirectory + "\\Upload\\" + newUser.Image.FileName;
                System.IO.File.Delete(Environment.CurrentDirectory + "\\Upload\\" + existingUser.ImageName);
                existingUser.Username = newUser.Username;
                existingUser.Password = newUser.Password;
                existingUser.ImageName = newUser.Image.FileName;
                await unitOfWork.UserRepository.UpdateUserAsync(existingUser);
                using (Stream fileStream = new FileStream(workingDirectory, FileMode.Create))
                {
                    await newUser.Image.CopyToAsync(fileStream);
                }
                await unitOfWork.SaveChangesAsync();
                return Ok($"User: {newUser.UserId} succesfully updated.");
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error.");
            }
        }

        [HttpPatch]
        [Route("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateUserAdminEndpoint([FromForm] UpdateUserNormalDTO newUser)
        {
            try
            {
                var existingUser = await unitOfWork.UserRepository.GetUserByIDAsync(newUser.UserId);
                if (existingUser == null)
                    return NotFound("User doesn't exist in the database.");
                string workingDirectory = Environment.CurrentDirectory + "\\Upload\\" + newUser.Image.FileName;
                System.IO.File.Delete(Environment.CurrentDirectory + "\\Upload\\" + existingUser.ImageName);
                existingUser.Username = newUser.Username;
                existingUser.Password = newUser.Password;
                existingUser.ImageName = newUser.Image.FileName;
                await unitOfWork.UserRepository.UpdateUserAsync(existingUser);
                using (Stream fileStream = new FileStream(workingDirectory, FileMode.Create))
                {
                    await newUser.Image.CopyToAsync(fileStream);
                }
                await unitOfWork.SaveChangesAsync();
                return Ok($"User: {newUser.UserId} succesfully updated.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error.");
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser([FromForm] Guid userID)
        {
            await unitOfWork.UserRepository.DeleteUserAsync(userID);
            var user = await unitOfWork.UserRepository.GetUserByIDAsync(userID);
            if(user.Reviews!=null)
            {
                foreach(Review rev in user.Reviews)
                {
                    await unitOfWork.ReviewRepository.DeleteReviewAsync(rev.Id);
                }
            }
            return Ok(unitOfWork.SaveChangesAsync());
        }
    }
}
