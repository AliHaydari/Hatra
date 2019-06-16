using Hatra.Entities.AuditableEntity;

namespace Hatra.Entities
{
    public class PageImage : IAuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Order { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
        public int Size { get; set; }
        public int PageId { get; set; }

        public virtual Page Page { get; set; }
    }
}
