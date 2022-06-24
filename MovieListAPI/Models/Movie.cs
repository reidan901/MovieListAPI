using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieListAPI.Models
{
    public class Movie :BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }
        [Required]
        [MaxLength(100)]
        public string Category { get; set; }
        [Column(TypeName = "decimal(1)")]
        public decimal Rating { get; set; }

        public List<Review>? Reviews { get; set; }
    }
}
