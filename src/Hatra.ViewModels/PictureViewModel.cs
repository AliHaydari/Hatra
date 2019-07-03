using System.ComponentModel.DataAnnotations;
using Hatra.Entities;
using Hatra.ViewModels.FileUpload;
using Microsoft.AspNetCore.Mvc;

namespace Hatra.ViewModels
{
    public class PictureViewModel
    {
        public PictureViewModel()
        {

        }

        public PictureViewModel(Picture picture)
        {
            Id = picture.Id;
            Path = picture.Path;

            Name = picture.Name;
            Size = picture.Size;
            Type = picture.Type;
            Url = picture.Url;
            DeleteUrl = picture.DeleteUrl;
            ThumbnailUrl = picture.ThumbnailUrl;
            DeleteType = picture.DeleteType;
            Extension = picture.Extension;

            FolderId = picture.FolderId;
            FolderName = picture.Folder?.Name;
        }

        public PictureViewModel(ViewDataUploadFilesResult viewDataUploadFilesResult)
        {
            Name = viewDataUploadFilesResult.name;
            Size = viewDataUploadFilesResult.size;
            Type = viewDataUploadFilesResult.type;
            Url = viewDataUploadFilesResult.url;
            DeleteUrl = viewDataUploadFilesResult.deleteUrl;
            ThumbnailUrl = viewDataUploadFilesResult.thumbnailUrl;
            DeleteType = viewDataUploadFilesResult.deleteType;
            Extension = viewDataUploadFilesResult.extension;
        }

        public PictureViewModel(ViewDataUploadFilesResult viewDataUploadFilesResult, int folderId)
        {
            FolderId = folderId;

            Name = viewDataUploadFilesResult.name;
            Size = viewDataUploadFilesResult.size;
            Type = viewDataUploadFilesResult.type;
            Url = viewDataUploadFilesResult.url;
            DeleteUrl = viewDataUploadFilesResult.deleteUrl;
            ThumbnailUrl = viewDataUploadFilesResult.thumbnailUrl;
            DeleteType = viewDataUploadFilesResult.deleteType;
            Extension = viewDataUploadFilesResult.extension;
        }

        [HiddenInput]
        public int Id { get; set; }

        [Display(Name = "مسیر")]
        public string Path { get; set; }

        public string Name { get; set; }
        public long Size { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string DeleteUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public string DeleteType { get; set; }
        public string Extension { get; set; }

        [HiddenInput]
        public int FolderId { get; set; }

        public string FolderName { get; set; }

        public FilesViewModel FilesViewModel { get; set; }
    }
}
