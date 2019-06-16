namespace Hatra.Entities
{
    public class Picture
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public int FolderId { get; set; }

        public Folder Folder { get; set; }
    }
}
