using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce.Data
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Range(0, 1000000)]
        [Column(TypeName ="decimal(18,2)")]
        public decimal Price { get; set; } = 0M;

        [Required]
        [ForeignKey("CategoryId")]
        public Category Category { get; set; } = new Category();

    }
}
