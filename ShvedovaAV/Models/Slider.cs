using System.ComponentModel.DataAnnotations.Schema;

namespace ShvedovaAV.Models
{
    public class Slider
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
