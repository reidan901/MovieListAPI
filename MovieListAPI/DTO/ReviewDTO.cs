namespace MovieListAPI.DTO
{
    public class ReviewDTO
    {
        public Guid UserID { get; set; }

        public Guid MovieID { get; set; }
        public string Description { get; set; }

        public decimal Rating { get; set; }
    }
}
