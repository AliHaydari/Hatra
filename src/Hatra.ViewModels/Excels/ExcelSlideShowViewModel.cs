using Hatra.Entities;

namespace Hatra.ViewModels.Excels
{
    public class ExcelSlideShowViewModel
    {
        public ExcelSlideShowViewModel()
        {

        }

        public ExcelSlideShowViewModel(SlideShow slideShow)
        {
            Id = slideShow.Id;
            Title = slideShow.Title;
            BriefDescription = slideShow.BriefDescription;
            Description = slideShow.Description;
            Image = slideShow.Image;
            Link1 = slideShow.Link1;
            Link2 = slideShow.Link2;
            Order = slideShow.Order;
            IsShow = slideShow.IsShow;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string BriefDescription { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public string Link1 { get; set; }

        public string Link2 { get; set; }

        public int Order { get; set; }

        public bool IsShow { get; set; }
    }
}
