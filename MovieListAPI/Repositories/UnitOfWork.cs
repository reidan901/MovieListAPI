namespace MovieListAPI.Repositories
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IReviewRepository ReviewRepository { get; }

        IMovieRepository MovieRepository { get; }
        public Task<bool> SaveChangesAsync();
    }
    public class UnitOfWork:IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IUserRepository UserRepository { get; set; }
        public IReviewRepository ReviewRepository { get; set; }

        public IMovieRepository MovieRepository { get; set; }

        public UnitOfWork ( AppDbContext efDbContext, 
            IUserRepository users,
            IReviewRepository reviews,
            IMovieRepository movies)
        {
            UserRepository = users;
            _context = efDbContext;
            ReviewRepository = reviews;
            MovieRepository = movies;
        }

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                var savedChanges = await _context.SaveChangesAsync();
                return savedChanges > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
