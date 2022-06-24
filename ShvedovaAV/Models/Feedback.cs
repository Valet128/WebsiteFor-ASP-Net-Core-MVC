using System.ComponentModel.DataAnnotations.Schema;

namespace ShvedovaAV.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public string? AuthorName { get; set; }
        public string? Text { get; set; }
        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
