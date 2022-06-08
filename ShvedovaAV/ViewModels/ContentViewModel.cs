using System.ComponentModel.DataAnnotations;

namespace ShvedovaAV.ViewModels
{
    public class ContentViewModel
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public IFormFile? VideoFile { get; set; }
        
    }
}
