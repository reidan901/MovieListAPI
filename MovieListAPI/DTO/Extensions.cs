using MovieListAPI.Models;

namespace MovieListAPI.DTO
{
    public static class Extensions
    {
        public static UserNormalAcessDTO AsDtoNormal(this User user)
        {
            return new UserNormalAcessDTO
            {
                id = user.Id,
                Username = user.Username,
                ImageName = user.ImageName,
                role = user.role.ToString(),
                Reviews = user.Reviews
            };
        }

        public static UserAdminAcessDTO AsDtoAdmin(this User user)
        {
            return new UserAdminAcessDTO
            {
                id = user.Id,
                Username = user.Username,
                Password = user.Password,
                ImageName = user.ImageName,
                role = user.role.ToString(),
                Reviews = user.Reviews
            };
        }
    }
}
