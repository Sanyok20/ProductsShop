using System.ComponentModel.DataAnnotations;

namespace Products.ViewModels
{
    public class ProductCreateVM
    {
        [Required(ErrorMessage = "Назва товару є обов'язковою")]
        [MaxLength(50, ErrorMessage = "Максимальна довжина 50 символів")]
        public string? Name { get; set; }
        public double Price { get; set; }
        public IFormFile? Image { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }

    }
}
