using System.ComponentModel.DataAnnotations;

namespace UsersProducts.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public int Price { get; set; }
    }
}
