﻿using System.Collections.Generic;
using Hatra.Entities.AuditableEntity;

namespace Hatra.Entities
{
    public class Category : IAuditableEntity
    {
        public Category()
        {
            Pages = new HashSet<Page>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsShow { get; set; }
        public string SlugUrl { get; set; }

        public virtual ICollection<Page> Pages { get; set; }
    }
}