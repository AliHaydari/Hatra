using Hatra.Entities.AuditableEntity;

namespace Hatra.Entities
{
    public class UsefulLink : IAuditableEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Link { get; set; }

        public int Order { get; set; }

        public bool IsShow { get; set; }

        public string Description { get; set; }
    }
}
