using System.ComponentModel.DataAnnotations;

namespace Products.ViewModels
{
    public class ProductUpdateVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Назва товару є обов'язковою")]
        [MaxLength(50, ErrorMessage = "Максимальна довжина 50 символів")]
        public string? Name { get; set; }
        public double Price { get; set; }
        public IFormFile? Image { get; set; }
        public string? Description { get; set; }
    }
}
