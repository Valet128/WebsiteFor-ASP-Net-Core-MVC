using System.ComponentModel.DataAnnotations;

namespace ShvedovaAV.ViewModels
{
    public class AddProductViewModel
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public string? Category { get; set; }
        [Required]
        public IFormFile? Image { get; set; }
    }
}
