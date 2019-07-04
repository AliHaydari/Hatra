namespace Hatra.ViewModels
{
    public class DriveInfoViewModel
    {
        public string Name { get; set; }

        public string DriveType { get; set; }

        public string VolumeLabel { get; set; }

        public string DriveFormat { get; set; }

        public long AvailableFreeSpace { get; set; }

        public long TotalFreeSpace { get; set; }

        public long TotalSize { get; set; }

        public string AvailableFreeSpaceInMb
        {
            get
            {
                try
                {
                    var result = ((AvailableFreeSpace / 1024) / 1024);
                    return result.ToString("##,###");
                }
                catch
                {
                    return "0";
                }
            }
        }
    }
}
