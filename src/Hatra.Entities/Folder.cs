using Hatra.Entities.AuditableEntity;
using System.Collections.Generic;

namespace Hatra.Entities
{
    public class Folder : IAuditableEntity
    {
        public Folder()
        {
            Pictures = new HashSet<Picture>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Picture> Pictures { get; set; }
    }
}
