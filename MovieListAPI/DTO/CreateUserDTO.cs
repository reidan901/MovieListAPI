namespace MovieListAPI.DTO
{
    public class LoginUserDTO
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
    public class CreateUserNormalDTO : LoginUserDTO
    {
        public IFormFile Image { get; set; }
    }

    public class CreateUserAdminDTO : LoginUserDTO
    {
        public IFormFile Image { get; set; }

        public string Role { get; set; }
    }
}