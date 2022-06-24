using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MovieListAPI.Models
{
    [Index(nameof(Username), IsUnique = true)]
    public class User : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(50)]
        public string Password { get; set; }

        [Required]
        public string ImageName { get; set; }

        public List<Review>? Reviews { get; set; }
    }
}
