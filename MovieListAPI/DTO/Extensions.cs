using MovieListAPI.Models;

namespace MovieListAPI.DTO
{
    public static class Extensions
    {
        public static UserAsNormalDTO AsDto(this User user)
        {
            return new UserAsNormalDTO
            {
                id = user.Id,
                Username = user.Username,
                ImageName = user.ImageName,
                role = user.role.ToString(),
                Reviews = user.Reviews
            };
        }
    }
}
