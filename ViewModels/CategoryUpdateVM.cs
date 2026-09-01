using System.ComponentModel.DataAnnotations;

namespace Products.ViewModels
{
    public class CategoryUpdateVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Назва категорії є обов'язковою")]
        [MaxLength(50, ErrorMessage = "Максимальна довжина 50 символів")]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
    }
}
