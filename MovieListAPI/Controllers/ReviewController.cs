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

        [HttpGet]
        [Authorize(Roles ="NormalUser,Admin")]
        public async Task<IEnumerable<ReviewDTO>> GetReviews()
        {
            return (await unitOfWork.ReviewRepository.GetReviewsAsync()).Select(r => r.AsDto()).ToList();
        }

        [HttpGet]
        [Authorize(Roles = "NormalUser,Admin")]
        [Route("{reviewID}")]
        public async Task<ReviewDTO> GetReviewByID(Guid reviewID)
        {
            return (await unitOfWork.ReviewRepository.GetReviewByIDAsync(reviewID)).AsDto();
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
                var movie = await unitOfWork.MovieRepository.GetMovieByIDAsync(review.MovieID);
                if (movie.Reviews != null)
                {
                    float average = review.Rating;
                    foreach (var movieReview in movie.Reviews)
                        average += movieReview.Rating;
                    average /= movie.Reviews.Count() + 1;
                    movie.Rating = average;
                }
                else
                    movie.Rating = review.Rating;
                await unitOfWork.MovieRepository.UpdateMovieAsync(movie);
                var newReview = new Review
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    Description = review.Description,
                    Rating = review.Rating,
                    UserID = review.UserID,
                    MovieID=review.MovieID,
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

        [HttpPatch]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateReview(UpdateReviewDTO review)
        {
            var existingReview = await unitOfWork.ReviewRepository.GetReviewByIDAsync(review.ReviewID);
            if (existingReview == null)
                return NotFound("No review exists with that ID.");
            var movie = await unitOfWork.MovieRepository.GetMovieByIDAsync(review.MovieID);
            movie.Rating = (movie.Rating * movie.Reviews.Count() + review.Rating - existingReview.Rating) / movie.Reviews.Count();
            existingReview.Description = review.Description;
            existingReview.Rating = review.Rating;
            await unitOfWork.ReviewRepository.UpdateReviewAsync(existingReview);
            return Ok(await unitOfWork.SaveChangesAsync());
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteReview([FromForm]Guid reviewID)
        {
            var existingReview = await unitOfWork.ReviewRepository.GetReviewByIDAsync(reviewID);
            existingReview.DeletedAt = DateTime.UtcNow;
            await unitOfWork.ReviewRepository.UpdateReviewAsync(existingReview);
            return Ok(await unitOfWork.SaveChangesAsync());
        }

    }
}
