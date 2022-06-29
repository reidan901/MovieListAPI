using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieListAPI.Models
{

    public enum UserRole
    {
        Admin,
        NormalUser
    }
    [Index(nameof(Username), IsUnique = true)]
    [Index(nameof(ImageName), IsUnique = true)]
    public class User : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(50)]
        public string Password { get; set; }

        [Column(TypeName = "nvarchar(24)")]
        [Required]
        public UserRole role { get; set; }

        [Required]
        public string ImageName { get; set; }

        public List<Review>? Reviews { get; set; }
    }
}
