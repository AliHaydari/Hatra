using DNTCommon.Web.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.IO;
using System.Threading.Tasks;

namespace Hatra.Controllers
{
    public class DownloadableContentsController : Controller
    {
        private readonly string _path;
        private const string ContentType = @"application/octet-stream";
        private const string TempPath = @"DownloadableFiles";

        public DownloadableContentsController(IHostingEnvironment hostingEnvironment)
        {
            _path = Path.Combine(hostingEnvironment.WebRootPath, TempPath);
        }

        [NoBrowserCache]
        [Route("download/{fileName}")]
        public async Task<IActionResult> Index(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return BadRequest();
            }

            //if (!Directory.Exists(_path))
            //{
            //    Directory.CreateDirectory(_path);
            //}

            var path = Path.Combine(_path, fileName);

            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(path, out var contentType))
            {
                contentType = ContentType;
            }

            var file = await System.IO.File.ReadAllBytesAsync(path);
            return File(file, contentType, fileName);
        }
    }
}