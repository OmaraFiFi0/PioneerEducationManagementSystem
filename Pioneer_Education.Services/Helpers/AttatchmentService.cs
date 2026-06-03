using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Pioneer_Education.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pioneer_Education.Services.Helpers
{
    public class AttatchmentService : IAttacehmentService
    {

        private readonly long _maxFileLength = 5 * 1024 * 1024; // 5MB
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".svg" };
        private readonly ILogger<AttatchmentService> _logger;

        public AttatchmentService(ILogger<AttatchmentService> logger)
        {
            _logger = logger;
        }

        public async Task<string?> UploadFileAsync(IFormFile file, string folderName)
        {

            try
            {
                if (file is null || file.Length == 0) return null;

                if (file.Length > _maxFileLength) return null;

                var extension = Path.GetExtension(file.FileName);

                if (!_allowedExtensions.Contains(extension)) return null;

                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", folderName);

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var fileName = Guid.NewGuid().ToString() + extension;

                var fullPath = Path.Combine(folderPath, fileName);

                using var FileStream = new FileStream(fullPath, FileMode.Create);

                await file.CopyToAsync(FileStream);

                return fileName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An UnExpected Error Happened while Uploading Image on Stream ");
                return null;
            }

        }


        public bool DeleteFile(string fileName, string folderName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName)) return false;

                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", folderName, fileName);

                if (File.Exists(folderPath))
                {
                    File.Delete(folderPath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An UnExpected Error Happened while Deleting Image  ");
                return false;
            }


        }
    }
}
