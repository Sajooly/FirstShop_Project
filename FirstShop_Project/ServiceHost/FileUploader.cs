using _0_Framework.Application;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace ServiceHost
{
    public class FileUploader : IFileUploader
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileUploader(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public string Upload(IFormFile file, string path)
        {
            if (file == null) return "";
            var fileName = $"{DateTime.Now.ToFileName()}-{file.FileName}";

            var directoryPath = $"{_webHostEnvironment.WebRootPath}/ProductPictures/{path}";
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            var filePath = $"{directoryPath}/{fileName}";

            //if (File.Exists(filePath))
            //    return $"{path}/{fileName}";

            using var output = File.Create(filePath);
            file.CopyToAsync(output);
            return $"{path}/{fileName}";
        }
    }
}
