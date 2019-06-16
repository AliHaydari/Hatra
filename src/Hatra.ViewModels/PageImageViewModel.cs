using Hatra.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Hatra.ViewModels
{
    public class PageImageViewModel
    {
        public PageImageViewModel()
        {

        }

        public PageImageViewModel(PageImage pageImage)
        {
            Id = pageImage.Id;
            Name = pageImage.Name;
            Order = pageImage.Order;
            Url = pageImage.Url;
            ThumbnailUrl = pageImage.ThumbnailUrl;
            Size = pageImage.Size;
            PageId = pageImage.PageId;
            PageName = pageImage.Page?.Title;
        }

        [HiddenInput]
        public int Id { get; set; }

        [Display(Name = "نام")]
        public string Name { get; set; }

        [Display(Name = "اولویت نمایش")]
        public int? Order { get; set; }

        public string Url { get; set; }

        public string ThumbnailUrl { get; set; }

        public int Size { get; set; }

        public int PageId { get; set; }

        public string PageName { get; set; }
    }
}
