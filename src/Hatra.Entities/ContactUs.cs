using Hatra.Entities.AuditableEntity;

namespace Hatra.Entities
{
    public class ContactUs : IAuditableEntity
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Subject { get; set; }

        public string Description { get; set; }

        public bool IsAnsered { get; set; }

        public string Answer { get; set; }
    }
}
