using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieListAPI.DTO;
using MovieListAPI.Models;
using MovieListAPI.Repositories;

namespace MovieListAPI.Controllers
{
    [ApiController]
    [Route("movie")]
    public class MovieController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration _config;

        public MovieController(IUnitOfWork uOF, IConfiguration config)
        {
            unitOfWork = uOF;
            _config = config;
        }

        [HttpGet]
        [Authorize(Roles = "NormalUser,Admin")]
        public async Task<IEnumerable<SendMovieDTO>> GetMovies()
        {
            var list1 = (await unitOfWork.MovieRepository.GetMoviesAsync());
            var list = list1.Select(movie => movie.AsDto());
            return list;
        }

        [HttpGet]
        [Route("{movieID}")]
        [Authorize(Roles = "NormalUser,Admin")]
        public async Task<ActionResult<SendMovieDTO>> GetMoviesByID(Guid movieID)
        {
            if (await unitOfWork.MovieRepository.GetMovieByIDAsync(movieID) == null)
                return NotFound("No movie exists with that ID.");
            return (await unitOfWork.MovieRepository.GetMovieByIDAsync(movieID)).AsDto();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddMovie([FromForm] MovieDTO movie)
        {
            try
            {
                MovieCategories category;
                switch(movie.Category)
                {
                    case "Horror":
                        category = MovieCategories.Horror;
                        break;
                    case "Action":
                        category = MovieCategories.Action;
                        break;
                    case "Comedy":
                        category = MovieCategories.Comedy;
                        break;
                    case "Thriller":
                        category = MovieCategories.Thriller;
                        break;
                        default:
                        return BadRequest("Category input must be: Horror, Action,Comdedy,Thriller");

                }
                var newMovie = new Movie
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    Name=movie.Name,
                    Description = movie.Description,
                    Category = category,
                    Rating = 0
                };
                await unitOfWork.MovieRepository.InsertMovieAsync(newMovie);
                return Ok(await unitOfWork.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error.");
            }
        }
    }
}
