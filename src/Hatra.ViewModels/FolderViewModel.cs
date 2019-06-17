using Hatra.ViewModels.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Hatra.Entities;

namespace Hatra.ViewModels
{
    public class FolderViewModel
    {
        public FolderViewModel()
        {

        }

        public FolderViewModel(Folder folder)
        {
            Id = folder.Id;
            Name = folder.Name;
        }

        [HiddenInput]
        public int Id { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "نام")]
        [Remote("ValidateName", "Folders",
            AdditionalFields = ViewModelConstants.AntiForgeryToken + "," + nameof(Id),
            HttpMethod = "POST")]
        [StringLength(50, ErrorMessage = "{0} باید حداقل {2} و حداکثر {1} حرف باشند.", MinimumLength = 2)]
        public string Name { get; set; }
    }
}
