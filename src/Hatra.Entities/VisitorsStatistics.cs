using System;

namespace Hatra.Entities
{
    public class VisitorsStatistics
    {
        public long Id { get; set; }

        public string UserAgent { get; set; }

        public string UserOs { get; set; }

        public string BrowserName { get; set; }

        public string DeviceName { get; set; }

        public string IpAddress { get; set; }

        public string PageViewed { get; set; }

        public DateTimeOffset VisitDate { get; set; }
    }
}
