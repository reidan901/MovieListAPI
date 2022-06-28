namespace MovieListAPI.DTO
{
    public class CreateUserNormalDTO
    {
        public IFormFile Image { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}