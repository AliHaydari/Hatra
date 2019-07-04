using Hatra.Entities;

namespace Hatra.ViewModels.Excels
{
    public class ExcelPageViewModel
    {
        public ExcelPageViewModel()
        {

        }

        public ExcelPageViewModel(Page page)
        {
            Id = page.Id;
            Title = page.Title;
            BriefDescription = page.BriefDescription;
            Body = page.Body;
            MetaDescription = page.MetaDescription;
            SlugUrl = page.SlugUrl;
            Image = page.Image;
            Order = page.Order;
            CategoryId = page.CategoryId;
            CategoryName = page.Category?.Name;
            CategorySlugUrl = page.Category?.SlugUrl;
            IsShow = page.IsShow;
            IsShowInLastContent = page.IsShowInLastContent;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string BriefDescription { get; set; }

        public string Body { get; set; }

        public string MetaDescription { get; set; }

        public string SlugUrl { get; set; }

        public string Image { get; set; }

        public int Order { get; set; }

        public int? CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string CategorySlugUrl { get; set; }

        public bool IsShow { get; set; }

        public bool IsShowInLastContent { get; set; }
    }
}
