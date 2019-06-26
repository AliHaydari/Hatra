using Hatra.Entities.AuditableEntity;

namespace Hatra.Entities
{
    public class Brand : IAuditableEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public string Link { get; set; }

        public string Description { get; set; }

        public bool IsShow { get; set; }
    }
}
