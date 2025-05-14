using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriEnergyConnect1.Models
{
    // Represents a product listed by a farmer in the system
    public class Product
    {
        // Primary key for the product
        public int Id { get; set; }

        // The name of the product (required, max 100 characters)
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100)]
        public string Name { get; set; }

        // The category of the product (e.g., Vegetables, Poultry, Equipment)
        [Required(ErrorMessage = "Category is required.")]
        public string Category { get; set; }

        // The price of the product (required, must be positive, formatted as currency)
        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 100000, ErrorMessage = "Price must be positive.")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        // The available quantity of the product (required, at least 1)
        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        // The date the product was produced (required)
        [Required(ErrorMessage = "Production date is required.")]
        [DataType(DataType.Date)]
        public DateTime ProductionDate { get; set; }

        // The date the product was listed (optional, but stored as a date)
        [DataType(DataType.Date)]
        public DateTime DateListed { get; set; }

        // Foreign key to the farmer who owns this product (required)
        [Required]
        public int FarmerId { get; set; }

        // Navigation property to the related Farmer entity
        public Farmer Farmer { get; set; }
    }
}