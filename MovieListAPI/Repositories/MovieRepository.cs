using Microsoft.EntityFrameworkCore;
using MovieListAPI.Models;

namespace MovieListAPI.Repositories
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetMoviesAsync();
        Task<Movie> GetMovieByIDAsync(Guid movieID);
        Task InsertMovieAsync(Movie movie);
        Task DeleteMovieAsync(Guid movieID);
        Task UpdateMovieAsync(Movie movie);
    }
    public class MovieRepository : IMovieRepository
    {
        private readonly AppDbContext _context;

        public MovieRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Movie>> GetMoviesAsync()
        {
            return await _context.Movies
                .Include(m => m.Reviews)
                .Where(movie => movie.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<Movie> GetMovieByIDAsync(Guid reviewID)
        {
            return await _context.Movies?
                .Include(m=> m.Reviews)
                .FirstOrDefaultAsync(e => e.Id == reviewID);
        }

        public async Task InsertMovieAsync(Movie movie)
        {
            await _context.Movies.AddAsync(movie);
            await Task.CompletedTask;
        }

        public async Task UpdateMovieAsync(Movie movie)
        {
            var itemToUpdate = await _context.Movies.FirstOrDefaultAsync(itm => itm.Id == movie.Id);
            if (itemToUpdate != null)
            {
                itemToUpdate.Id = movie.Id;
                itemToUpdate.Description = movie.Description;
            }
        }

        public async Task DeleteMovieAsync(Guid movieID)
        {
            var item = await _context.Reviews.FirstOrDefaultAsync(us => us.Id == movieID);
            if (item != null)
                item.DeletedAt = DateTime.UtcNow;
        }
    }
}
