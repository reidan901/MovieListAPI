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

        public static SendReviewDTO AsDto(this Review review)
        {
            return new SendReviewDTO
            {
                ReviewId = review.Id,
                MovieID = review.MovieID,
                UserID = review.UserID,
                Description = review.Description,
                Rating = review.Rating,
            };
        }

        public static SendMovieDTO AsDto(this Movie movie)
        {
            return new SendMovieDTO
            {
                MovieID = movie.Id,
                Description = movie.Description,
                Category = movie.Category.ToString(),
                Rating = movie.Rating,
                Name = movie.Name,
                Reviews = (List<SendReviewDTO>)movie.Reviews?.Select(movie => movie.AsDto())
            };
        }
    }
}
