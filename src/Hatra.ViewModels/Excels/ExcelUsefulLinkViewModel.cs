using Hatra.Entities;

namespace Hatra.ViewModels.Excels
{
    public class ExcelUsefulLinkViewModel
    {
        public ExcelUsefulLinkViewModel()
        {

        }

        public ExcelUsefulLinkViewModel(UsefulLink usefulLink)
        {
            Id = usefulLink.Id;
            Name = usefulLink.Name;
            Link = usefulLink.Link;
            Order = usefulLink.Order;
            Description = usefulLink.Description;
            IsShow = usefulLink.IsShow;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Link { get; set; }

        public int Order { get; set; }

        public bool IsShow { get; set; }

        public string Description { get; set; }
    }
}
