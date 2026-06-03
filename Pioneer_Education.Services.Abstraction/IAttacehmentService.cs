using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pioneer_Education.Services.Abstraction
{
    public interface IAttacehmentService
    {
        Task<string?> UploadFileAsync(IFormFile file, string folderName);

        bool DeleteFile(string fileName, string folderName);

    }
}
