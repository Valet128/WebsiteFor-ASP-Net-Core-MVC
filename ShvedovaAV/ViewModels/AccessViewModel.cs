using ShvedovaAV.Models;
using System.ComponentModel.DataAnnotations;

namespace ShvedovaAV.ViewModels
{
    public class AccessViewModel
    {
        public List<User>? Users { get; set; }
        public List<ContentAccess>? ContentAccesses { get; set; }
        
    }
}
