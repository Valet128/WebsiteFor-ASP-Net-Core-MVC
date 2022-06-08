
namespace ShvedovaAV.Controllers
{
    public class TestController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Test(IFormFile file)
        {
            string path = "wwwroot/Files/Videos/Content/" + file.FileName;
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
               await file.CopyToAsync(fileStream);
            }
            return View();
        }   
    }
}

