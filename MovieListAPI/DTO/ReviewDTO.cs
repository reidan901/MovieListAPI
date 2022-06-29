namespace MovieListAPI.DTO
{
    public class ReviewDTO
    {
        public Guid UserID { get; set; }

        public Guid MovieID { get; set; }
        public string Description { get; set; }

        public float Rating { get; set; }
    }

    public class SendReviewDTO:ReviewDTO
    {
        public Guid ReviewId { get; set; }
    }
}
