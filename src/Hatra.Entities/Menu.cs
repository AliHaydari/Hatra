using Hatra.Entities.AuditableEntity;
using Hatra.Entities.Enums;
using System.Collections.Generic;

namespace Hatra.Entities
{
    public class Menu : IAuditableEntity
    {
        public Menu()
        {
            
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public int? ParentId { get; set; }
        public int Order { get; set; }
        public MenuType Type { get; set; }
        public bool IsShow { get; set; }
        public bool IsMegaMenu { get; set; }

        public virtual ICollection<Menu> SubMenus { get; set; }
        public virtual Menu ParentMenu { get; set; }
    }
}
