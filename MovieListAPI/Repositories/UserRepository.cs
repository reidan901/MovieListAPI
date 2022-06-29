using Microsoft.EntityFrameworkCore;
using MovieListAPI.Models;
namespace MovieListAPI.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserByIDAsync(Guid userID);
        Task InsertUserAsync(User user);
        Task DeleteUserAsync(Guid userID);
        Task UpdateUserAsync(User user);
        Task<User> GetUserByUserNameAsync(string name);
    }
    public class UserRepository:IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await  _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIDAsync(Guid userID)
        {
            return await _context.Users.Include(e => e.Reviews).FirstOrDefaultAsync(e=>e.Id==userID);
        }

        public async Task InsertUserAsync(User user)
        {
            _context.Users.AddAsync(user);
            await Task.CompletedTask;
        }

        public async Task UpdateUserAsync(User user)
        {
            var itemToUpdate = await _context.Users.FirstOrDefaultAsync(itm => itm.Id == user.Id);
            if(itemToUpdate!=null)
            {
                itemToUpdate.Id = user.Id;
                itemToUpdate.Username = user.Username;
                itemToUpdate.Password = user.Password;
                itemToUpdate.Reviews = user.Reviews;
                itemToUpdate.role = user.role;
                itemToUpdate.ImageName=user.ImageName;

            }
        }

        public async Task DeleteUserAsync(Guid userID)
        {
            var item = await _context.Users.FirstOrDefaultAsync(us => us.Id == userID);
            if (item != null)
                item.DeletedAt = DateTime.UtcNow;
        }

        public async Task<User> GetUserByUserNameAsync(string name)
        {
            return await _context.Users.FirstOrDefaultAsync(us => us.Username==name);
        }
    }
}
