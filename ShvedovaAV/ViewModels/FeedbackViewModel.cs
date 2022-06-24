using System.ComponentModel.DataAnnotations;

namespace ShvedovaAV.ViewModels
{
    public class FeedbackViewModel
    {
        public int Id { get; set; }
        [Required]
        public string? AuthorName { get; set; }
        [Required]
        public string? Text { get; set; }
        [Required]
        public string? Image { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
