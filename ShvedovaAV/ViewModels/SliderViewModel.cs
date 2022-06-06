using System.ComponentModel.DataAnnotations;

namespace ShvedovaAV.ViewModels
{
    public class SliderViewModel
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public string? Image { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
