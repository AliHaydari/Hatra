using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Hatra.Common.Constants;
using Hatra.ViewModels.FileUpload;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Hatra.FileUpload
{
    public class CommandResult
    {
        public List<ViewDataUploadFilesResult> FileResults { get; private set; } = new List<ViewDataUploadFilesResult>();
    }

    public class FileUploadUtilities
    {
        //public class Command : IRequest<CommandResult>
        //{
        //    [BindNever]
        //    public HttpContext HttpContext { get; set; }

        //    public List<IFormFile> Files { get; private set; } = new List<IFormFile>();
        //}

        private readonly FilesHelper _filesHelper;

        public FileUploadUtilities(FilesHelper filesHelper)
        {
            _filesHelper = filesHelper;
        }

        public async Task<CommandResult> Handle(HttpContext httpContext, List<IFormFile> files, CancellationToken cancellationToken)
        {
            var result = new CommandResult();

            var partialFileName = httpContext.Request.Headers["X-File-Name"];
            if (string.IsNullOrWhiteSpace(partialFileName))
            {
                await UploadWholeFileAsync(files, result);
            }
            else
            {
                UploadPartialFile(httpContext, partialFileName);
            }

            return result;
        }

        private static readonly HashSet<string> _allowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                ".gif",
                ".jpeg",
                ".jpg",
                ".png",
                ".svg",
                ".tif",
                ".pdf",
                ".xls",
                ".xlsx",
                ".zip",
                ".rar",
                ".apk",
            };

        private async Task UploadWholeFileAsync(List<IFormFile> files, CommandResult result)
        {
            //const int THUMB_WIDTH = 80;
            //const int THUMB_HEIGHT = 80;
            //const int NORMAL_IMAGE_MAX_WIDTH = 540;
            const string THUMBS_FOLDER_NAME = "thumbs";

            // Ensure the storage root exists.
            Directory.CreateDirectory(_filesHelper.StorageRootPath);

            foreach (var file in files)
            {
                bool isImage;

                string fileName = string.Empty;

                var extension = Path.GetExtension(file.FileName);

                isImage = Common.Extensions.ImageExtensions.IsImage(extension);

                if (isImage)
                {
                    fileName = Guid.NewGuid().ToString("N");
                }
                else
                {
                    fileName = file.FileName;
                }

                if (!_allowedExtensions.Contains(extension))
                {
                    // This is not a supported image type.
                    throw new InvalidOperationException($"Unsupported image type: {extension}. The supported types are: {string.Join(", ", _allowedExtensions)}");
                }

                if (file.Length > 0L)
                {
                    string fullPath = string.Empty;

                    if (isImage)
                    {
                        fullPath = Path.Combine(_filesHelper.StorageRootPath, Path.GetFileName(fileName + extension));
                        using (var fs = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(fs);
                        }
                    }
                    else
                    {
                        fullPath = Path.Combine(_filesHelper.StorageRootPath, Path.GetFileName(fileName));
                        using (var fs = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(fs);
                        }
                    }



                    if (isImage)
                    {
                        //
                        // Create an 80x80 thumbnail.
                        //

                        //var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
                        var thumbName =
                            $"{fileName}{ImageConstants.ThumbWidth80}x{ImageConstants.ThumbHeight80}{extension}";
                        var thumbPath = Path.Combine(_filesHelper.StorageRootPath, THUMBS_FOLDER_NAME, thumbName);

                        // Create the thumnail directory if it doesn't exist.
                        Directory.CreateDirectory(Path.GetDirectoryName(thumbPath));

                        using (var thumb = Image.Load(ResizeImage(fullPath, Convert.ToInt32(ImageConstants.ThumbWidth80),
                            Convert.ToInt32(ImageConstants.ThumbHeight80))))
                        {
                            thumb.Save(thumbPath);
                        }

                        using (var thumb = Image.Load(ResizeImage(fullPath, Convert.ToInt32(ImageConstants.ThumbWidth370),
                            Convert.ToInt32(ImageConstants.ThumbHeight180))))
                        {
                            thumb.Save(Path.Combine(_filesHelper.StorageRootPath, THUMBS_FOLDER_NAME,
                                $"{fileName}{ImageConstants.ThumbWidth370}x{ImageConstants.ThumbHeight180}{extension}"));
                        }

                        using (var thumb = Image.Load(ResizeImage(fullPath, Convert.ToInt32(ImageConstants.ThumbWidth90),
                            Convert.ToInt32(ImageConstants.ThumbHeight81))))
                        {
                            thumb.Save(Path.Combine(_filesHelper.StorageRootPath, THUMBS_FOLDER_NAME,
                                $"{fileName}{ImageConstants.ThumbWidth90}x{ImageConstants.ThumbHeight81}{extension}"));
                        }
                    }

                    // If the image is wider than 540px, resize it so that it is 540px wide. Otherwise, upload a copy of the original.
                    //using (var originalImage = Image.Load(fullPath))
                    //{
                    //    if (originalImage.Width > NORMAL_IMAGE_MAX_WIDTH)
                    //    {
                    //        // Resize it so that the max width is 540px. Maintain the aspect ratio.
                    //        var newHeight = originalImage.Height * NORMAL_IMAGE_MAX_WIDTH / originalImage.Width;

                    //        var normalImageName = $"{fileNameWithoutExtension}{NORMAL_IMAGE_MAX_WIDTH}x{newHeight}{extension}";
                    //        var normalImagePath = Path.Combine(_filesHelper.StorageRootPath, normalImageName);

                    //        using (var normalImage = Image.Load(ResizeImage(fullPath, NORMAL_IMAGE_MAX_WIDTH, newHeight)))
                    //        {
                    //            normalImage.Save(normalImagePath);
                    //        }
                    //    }
                    //}
                }
                else
                {

                }

                result.FileResults.Add(UploadResult(isImage ? fileName + extension : fileName, extension, file.Length));
            }
        }

        private byte[] ResizeImage(string localTempFilePath, int width, int height)
        {
            try
            {
                using (var originalImage = Image.Load(localTempFilePath))
                using (var thumbnailImage = originalImage.Clone())
                using (var thumbnailStream = new MemoryStream())
                {
                    var extension = Path.GetExtension(localTempFilePath);
                    IImageFormat format = originalImage.GetConfiguration().ImageFormatsManager.FindFormatByFileExtension(extension);
                    IImageEncoder encoder = originalImage.GetConfiguration().ImageFormatsManager.FindEncoder(format);

                    if (Common.Extensions.ImageExtensions.IsJpeg(extension))
                    {
                        // It's a JPEG, so ensure we're maintaining quality.
                        encoder = new JpegEncoder { Quality = 90 };
                    }

                    // Resize the image.
                    thumbnailImage.Mutate(op =>
                    {
                        op.Resize(width, height);
                    });

                    // Save it to the stream.
                    thumbnailImage.Save(thumbnailStream, encoder);

                    // Return the bytes to save to disk.
                    return thumbnailStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Unhandled error trying to resize local image '{localTempFilePath}' to Width={width}px, Height={height}px", ex);
            }
        }

        private void UploadPartialFile(HttpContext httpContext, string partialFileName)
        {
            throw new NotImplementedException();
        }

        private ViewDataUploadFilesResult UploadResult(string fileName, string fileExtension, long fileSizeInBytes)
        {
            var getType = MimeMapping.GetMimeMapping(fileName);

            var result = new ViewDataUploadFilesResult()
            {
                name = fileName,
                size = fileSizeInBytes,
                type = getType,
                url = _filesHelper.UrlBase + fileName,
                deleteUrl = _filesHelper.DeleteUrl + fileName,
                thumbnailUrl = Common.Extensions.ImageExtensions.IsImage(fileExtension) ? _filesHelper.CheckThumb(getType, fileName) : null,
                deleteType = _filesHelper.DeleteType,
                extension = fileExtension,
            };

            return result;
        }
    }

}
