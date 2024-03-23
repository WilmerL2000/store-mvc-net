﻿using System.ComponentModel.DataAnnotations;

namespace BestStoreMVC.Models
{
    public class ProductDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required, MaxLength(100)]
        public string Brand { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required, MaxLength(100)]
        public string Category { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; }
        public IFormFile? ImageFile { get; set; } 
    }
}
