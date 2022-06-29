namespace MovieListAPI.DTO
{
    public class UpdateUserNormalDTO
    {
        public Guid UserId { get; set; }    
        public string Username { get; set; }
        public string Password { get; set; }
        public IFormFile Image { get; set; }
    }
}
