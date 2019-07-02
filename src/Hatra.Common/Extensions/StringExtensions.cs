using System.Text.RegularExpressions;

namespace Hatra.Common.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveHtmlTags(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return string.Empty;
            }

            var noHtml = Regex.Replace(str, @"<[^>]+>|&nbsp;", "").Trim();
            var noHtmlNormalised = Regex.Replace(noHtml, @"\s{2,}", " ");

            return noHtmlNormalised;
        }

        public static bool ContainsExcel(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }

            if (str.Contains(".xls") || str.Contains(".xlsx"))
            {
                return true;
            }

            return false;
        }
    }
}
