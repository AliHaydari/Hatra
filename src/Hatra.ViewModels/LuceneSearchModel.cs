namespace Hatra.ViewModels
{
    public class LuceneSearchModel
    {
        public int? PageId { get; set; }
        public string Title { get; set; }
        public string BriefDescription { get; set; }
        public string Body { get; set; }
        public string SlugUrl { get; set; }
        public string Image { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool IsShow { get; set; }

        public string ImageName => Image?.Remove(0, 21).Substring(0, 32);
    }
}
