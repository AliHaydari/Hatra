using Hatra.Entities;

namespace Hatra.ViewModels.Excels
{
    public class ExcelBrandViewModel
    {
        public ExcelBrandViewModel()
        {

        }

        public ExcelBrandViewModel(Brand brand)
        {
            Id = brand.Id;
            Name = brand.Name;
            Image = brand.Image;
            Link = brand.Link;
            Description = brand.Description;
            IsShow = brand.IsShow;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public string Link { get; set; }

        public string Description { get; set; }

        public bool IsShow { get; set; }
    }
}
