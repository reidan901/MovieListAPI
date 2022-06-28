using MovieListAPI.Models;

namespace MovieListAPI.DTO
{
    public class UserAsNormalDTO
    {
        public Guid id { get; set; }
        public string Username { get; set; }

        public string role { get; set; }

        public string ImageName { get; set; }

        public List<Review>? Reviews { get; set; }
    }
}
