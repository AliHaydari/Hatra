using System.ComponentModel.DataAnnotations;
using Hatra.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Hatra.ViewModels
{
    public class PictureViewModel
    {
        public PictureViewModel()
        {

        }

        public PictureViewModel(Picture picture)
        {
            Id = picture.Id;
            Path = picture.Path;
            FolderId = picture.FolderId;
            FolderName = picture.Folder?.Name;
        }

        [HiddenInput]
        public int Id { get; set; }

        [Display(Name = "مسیر")]
        public string Path { get; set; }

        public int FolderId { get; set; }

        public string FolderName { get; set; }
    }
}
