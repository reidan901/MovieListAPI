namespace MovieListAPI.DTO
{
    public class MovieDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public string Category { get; set; }
    }

    public class SendMovieDTO:MovieDTO
    {
        public Guid MovieID { get; set; }
        public float Rating { get; set; }

        public List<SendReviewDTO>? Reviews { get; set; }
    }
}
