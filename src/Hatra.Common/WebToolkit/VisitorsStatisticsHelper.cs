using System.Collections.Generic;
using System.Linq;
using UAParser;

namespace Hatra.Common.WebToolkit
{
    public class VisitorsStatisticsHelper
    {
        public static Parser Parser => Parser.GetDefault();

        //list of bots and crawlers
        private static readonly List<string> KnownCrawlers = new List<string>
        {
            "bot","crawler","baiduspider","80legs","ia_archiver","ahrefsBot","twitterbot",
            "yoozbot","yandexBot","bitlybot","other", "sogou web spider", "python requests",
            "voyager","curl","wget","yahoo! slurp","mediapartners-google", "mj12bot",
            "seznamBot", "Sogou web spider", "360Spider", "sogouwebspider"
        };

        //detect the crawlers and bots
        public static bool IsBotOrCrawler(string agent)
        {
            agent = agent.ToLower();
            return KnownCrawlers.Any(crawler => agent.Contains(crawler) || agent.Equals(crawler));
        }

        public static OS GetUserOsName(string userAgent)
        {
            ClientInfo c = Parser.Parse(userAgent);
            return c.OS;
        }

        public static UserAgent GetUserBrowserName(string userAgent)
        {
            ClientInfo c = Parser.Parse(userAgent);
            return c.UA;
        }

        public static Device GetUserDeviceName(string userAgent)
        {
            ClientInfo c = Parser.Parse(userAgent);
            return c.Device;
        }
    }
}
