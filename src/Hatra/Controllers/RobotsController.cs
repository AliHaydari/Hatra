using Hatra.Common.GuardToolkit;
using Hatra.Services.Contracts;
using Hatra.ViewModels.Identity.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Atom;
using Microsoft.SyndicationFeed.Rss;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Hatra.Controllers
{
    public class RobotsController : Controller
    {
        private readonly IPageService _pageService;
        private readonly IOptionsSnapshot<ShowingSettingSite> _settings;

        public RobotsController(IPageService pageService, IOptionsSnapshot<ShowingSettingSite> settings)
        {
            _pageService = pageService;
            _pageService.CheckArgumentIsNull(nameof(_pageService));

            _settings = settings;
            _settings.CheckArgumentIsNull(nameof(_settings));
        }

        [Route("/robots.txt")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public string RobotsTxt()
        {
            string host = Request.Scheme + "://" + Request.Host;
            var sb = new StringBuilder();
            sb.AppendLine("User-agent: *");
            sb.AppendLine("Disallow:");
            sb.AppendLine($"sitemap: {host}/sitemap.xml");

            return sb.ToString();
        }

        [Route("/sitemap.xml")]
        public async Task SitemapXml()
        {
            string host = Request.Scheme + "://" + Request.Host;

            Response.ContentType = "application/xml";

            using (var xml = XmlWriter.Create(Response.Body, new XmlWriterSettings { Indent = true }))
            {
                xml.WriteStartDocument();
                xml.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

                var posts = await _pageService.GetAllAsync(int.MaxValue);

                foreach (var pageViewModel in posts)
                {
                    var lastMod = new[] { pageViewModel.CreatedDateTimeInDateTime, pageViewModel.ModifiedDateTimeInDateTime };

                    xml.WriteStartElement("url");
                    xml.WriteElementString("loc", host + $@"page/{pageViewModel.Id}/{pageViewModel.SlugUrl}");
                    xml.WriteElementString("lastmod", lastMod.Max().ToString("yyyy-MM-ddThh:mmzzz"));
                    xml.WriteEndElement();
                }

                xml.WriteEndElement();
            }
        }

        [Route("/rsd.xml")]
        public void RsdXml()
        {
            string host = Request.Scheme + "://" + Request.Host;

            Response.ContentType = "application/xml";
            Response.Headers["cache-control"] = "no-cache, no-store, must-revalidate";

            using (var xml = XmlWriter.Create(Response.Body, new XmlWriterSettings { Indent = true }))
            {
                xml.WriteStartDocument();
                xml.WriteStartElement("rsd");
                xml.WriteAttributeString("version", "1.0");

                xml.WriteStartElement("service");

                xml.WriteElementString("enginename", _settings.Value.EnglishSiteName);
                xml.WriteElementString("enginelink", _settings.Value.SiteUrl);
                xml.WriteElementString("homepagelink", host);

                xml.WriteStartElement("apis");
                xml.WriteStartElement("api");
                xml.WriteAttributeString("name", "MetaWeblog");
                xml.WriteAttributeString("preferred", "true");
                xml.WriteAttributeString("apilink", host + "/metaweblog");
                xml.WriteAttributeString("blogid", "1");

                xml.WriteEndElement(); // api
                xml.WriteEndElement(); // apis
                xml.WriteEndElement(); // service
                xml.WriteEndElement(); // rsd
            }
        }

        [Route("/feed/{type}")]
        public async Task Rss(string type)
        {
            Response.ContentType = "application/xml";
            string host = Request.Scheme + "://" + Request.Host;

            using (XmlWriter xmlWriter = XmlWriter.Create(Response.Body, new XmlWriterSettings() { Async = true, Indent = true }))
            {
                var pageViewModels = await _pageService.GetAllAsync(10);
                var writer = await GetWriter(type, xmlWriter, pageViewModels.Max(p => p.CreatedDateTimeInDateTime));

                foreach (var pageViewModel in pageViewModels)
                {
                    var item = new AtomEntry
                    {
                        Title = pageViewModel.Title,
                        Description = pageViewModel.BriefDescription,
                        Id = host + $@"page/{pageViewModel.Id}/{pageViewModel.SlugUrl}",
                        Published = pageViewModel.CreatedDateTimeInDateTime,
                        LastUpdated = pageViewModel.ModifiedDateTimeInDateTime,
                        ContentType = "html",
                    };

                    //foreach (string category in pageViewModel.Categories)
                    //{
                    //    item.AddCategory(new SyndicationCategory(category));
                    //}

                    item.AddContributor(new SyndicationPerson(_settings.Value.Owner, _settings.Value.Email));
                    item.AddLink(new SyndicationLink(new Uri(item.Id)));

                    await writer.Write(item);
                }
            }
        }

        private async Task<ISyndicationFeedWriter> GetWriter(string type, XmlWriter xmlWriter, DateTime updated)
        {
            string host = Request.Scheme + "://" + Request.Host + "/";

            if (type.Equals("rss", StringComparison.OrdinalIgnoreCase))
            {
                var rss = new RssFeedWriter(xmlWriter);
                await rss.WriteTitle(_settings.Value.EnglishSiteName);
                await rss.WriteDescription(_settings.Value.Description);
                await rss.WriteGenerator(_settings.Value.EnglishSiteName);
                await rss.WriteValue("link", host);
                return rss;
            }

            var atom = new AtomFeedWriter(xmlWriter);
            await atom.WriteTitle(_settings.Value.EnglishSiteName);
            await atom.WriteId(host);
            await atom.WriteSubtitle(_settings.Value.Description);
            await atom.WriteGenerator(_settings.Value.EnglishSiteName, _settings.Value.SiteUrl, "1.0");
            await atom.WriteValue("updated", updated.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            return atom;
        }
    }
}