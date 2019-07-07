using Hatra.Entities.AuditableEntity;

namespace Hatra.Entities
{
    public class StaticContent : IAuditableEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        public int Order { get; set; }

        public bool IsShow { get; set; }
    }
}
