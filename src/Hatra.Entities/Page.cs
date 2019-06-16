using Hatra.Entities.AuditableEntity;
using System.Collections.Generic;

namespace Hatra.Entities
{
    public class Page : IAuditableEntity
    {
        public Page()
        {
            Images = new HashSet<PageImage>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string BriefDescription { get; set; }
        public string Body { get; set; }
        public string MetaDescription { get; set; }
        public string SlugUrl { get; set; }
        public int ViewNumber { get; set; }
        public string Image { get; set; }
        public int Order { get; set; }
        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<PageImage> Images { get; set; }
    }
}
