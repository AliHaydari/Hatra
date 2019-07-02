using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Hatra.ViewModels
{
    public class ImportFromExcelViewModel
    {
        [Required(ErrorMessage = "(*)")]
        [Display(Name = "انتخاب فایل")]
        public IFormFile File { get; set; }
    }
}
