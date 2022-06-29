using Microsoft.EntityFrameworkCore;
using MovieListAPI.Models;
namespace MovieListAPI.Repositories
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetReviewsAsync();
        Task<Review> GetReviewByIDAsync(Guid userID);
        Task InsertReviewAsync(Review review);
        Task DeleteReviewAsync(Guid userID);
        Task UpdateReviewAsync(Review review);
    }
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetReviewsAsync()
        {
            return await _context.Reviews.ToListAsync();
        }

        public async Task<Review> GetReviewByIDAsync(Guid reviewID)
        {
            return await _context.Reviews.FirstOrDefaultAsync(e => e.Id == reviewID);
        }

        public async Task InsertReviewAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await Task.CompletedTask;
        }

        public async Task UpdateReviewAsync(Review review)
        {
            var itemToUpdate = await _context.Reviews.FirstOrDefaultAsync(itm => itm.Id == review.Id);
            if (itemToUpdate != null)
            {
                itemToUpdate.Id = review.Id;
                itemToUpdate.Description = review.Description;
                itemToUpdate.Rating = review.Rating;
            }
        }

        public async Task DeleteReviewAsync(Guid reviewID)
        {
            var item = await _context.Reviews.FirstOrDefaultAsync(us => us.Id == reviewID);
            if (item != null)
                item.DeletedAt = DateTime.UtcNow;
        }
    }
}
