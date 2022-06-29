using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieListAPI.Models
{
    public class Review : BaseEntity
    {
        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public float Rating { get; set; }

        public Guid UserID { get; set; }

        public User User { get; set; }

        public Guid MovieID { get; set; }

        public Movie Movie { get; set; }
    }
}
