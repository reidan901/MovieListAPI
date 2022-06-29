using MovieListAPI.Models;

namespace MovieListAPI.DTO
{
    public class UserAdminAcessDTO
    {
        public Guid id { get; set; }
        public string Username { get; set; }

        public string Password { get; set; }

        public string role { get; set; }

        public string ImageName { get; set; }

        public List<Review>? Reviews { get; set; }
    }
    public class UserNormalAcessDTO
    {
        public Guid id { get; set; }
        public string Username { get; set; }

        public string role { get; set; }

        public string ImageName { get; set; }

        public List<Review>? Reviews { get; set; }
    }
}
