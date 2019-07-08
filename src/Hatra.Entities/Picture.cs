using Hatra.Entities.AuditableEntity;

namespace Hatra.Entities
{
    public class Picture : IAuditableEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public long Size { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string DeleteUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public string DeleteType { get; set; }
        public string Extension { get; set; }

        public string Path { get; set; }
        public int FolderId { get; set; }

        public Folder Folder { get; set; }
    }
}
