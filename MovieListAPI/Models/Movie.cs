﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieListAPI.Models
{

    public enum MovieCategories
    {
        Action,
        Horror,
        Comedy,
        Thriller,
        All
    }
    public class Movie :BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(24)")]
        public MovieCategories Category { get; set; }
        [Column(TypeName = "decimal(1)")]
        public decimal Rating { get; set; }

        public List<Review>? Reviews { get; set; }
    }
}
