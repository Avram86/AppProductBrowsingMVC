using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce.Models
{
    public class CategoryViewModel
    {
        [Key]
        [Required]
        public int ID { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}
