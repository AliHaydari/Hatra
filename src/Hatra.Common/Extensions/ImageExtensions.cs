using System;

namespace Hatra.Common.Extensions
{
    public static class ImageExtensions
    {
        public static bool IsImage(string fileExtension)
        {
            return ".jpeg".Equals(fileExtension, StringComparison.OrdinalIgnoreCase)
                   || ".jpg".Equals(fileExtension, StringComparison.OrdinalIgnoreCase)
                   || ".png".Equals(fileExtension, StringComparison.OrdinalIgnoreCase)
                   || ".gif".Equals(fileExtension, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsJpeg(string fileExtension)
        {
            return ".jpeg".Equals(fileExtension, StringComparison.OrdinalIgnoreCase)
                   || ".jpg".Equals(fileExtension, StringComparison.OrdinalIgnoreCase);
        }
    }
}
