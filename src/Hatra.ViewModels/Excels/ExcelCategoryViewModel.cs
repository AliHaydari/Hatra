using Hatra.Entities;

namespace Hatra.ViewModels.Excels
{
    public class ExcelCategoryViewModel
    {
        public ExcelCategoryViewModel()
        {
            
        }

        public ExcelCategoryViewModel(Category category)
        {
            Id = category.Id;
            Name = category.Name;
            Description = category.Description;
            IsShow = category.IsShow;
            SlugUrl = category.SlugUrl;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsShow { get; set; }

        public string SlugUrl { get; set; }
    }
}
