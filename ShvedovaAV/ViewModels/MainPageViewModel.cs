using ShvedovaAV.Models;
using System.Collections;

namespace ShvedovaAV.ViewModels
{
    public class MainPageViewModel
    {
        public List<Product>? Products { get; set; }
        public List<Slider>? Sliders { get; set; }
        public List<Feedback>? Feedbacks { get; set; }
    }
}
