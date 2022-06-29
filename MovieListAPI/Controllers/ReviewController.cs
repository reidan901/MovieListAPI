using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieListAPI.DTO;
using MovieListAPI.Models;
using MovieListAPI.Repositories;

namespace MovieListAPI.Controllers
{
    [ApiController]
    [Route("review")]
    public class ReviewController:ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration _config;

        public ReviewController(IUnitOfWork uOF, IConfiguration config)
        {
            unitOfWork = uOF;
            _config = config;
        }

        [HttpPost]
        [Authorize(Roles ="NormalUser,Admin")]
        public async Task<ActionResult> AddReview([FromForm]ReviewDTO review)
        {
            try
            {
                var user = await unitOfWork.UserRepository.GetUserByIDAsync(review.UserID);
                if (user == null)
                    return BadRequest("No users exist with provided ID.");
                var newReview = new Review
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    Description = review.Description,
                    Rating = review.Rating,
                    UserID = review.UserID,
                    User = user
                };
                await unitOfWork.ReviewRepository.InsertReviewAsync(newReview);
                return Ok(await unitOfWork.SaveChangesAsync());
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error.");
            }
        }

    }
}
