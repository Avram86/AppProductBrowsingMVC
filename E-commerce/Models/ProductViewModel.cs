using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        [Range(0, 1000000)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }



        //public List<CategoryViewModel> AvailableCategories { get; set; } = new();
        public List<SelectListItem> AvailableCategories { get; set; } = new();
        public int SelectedCategoryId { get; set; }



        //prod 1=> 1 categ
        public CategoryViewModel Category { get; set; } = new();
    }
}
