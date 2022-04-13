using HisaabKaro.Models.Common;
using HisabKaroDBContext;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HisaabKaro.Services
{
    public class FileUploadServices
    {
        private readonly IWebHostEnvironment _environment;

        public FileUploadServices(IWebHostEnvironment Environment)
        {
            _environment = Environment;
        }

       

        public Result Upload(Models.Common.File.Upload objFile)
        {
            using (HisabKaroDBDataContext context = new HisabKaroDBDataContext())
            {
                if (objFile.files.Length > 0)
                {
                    if (!Directory.Exists(_environment.WebRootPath + "\\Upload\\"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "\\Upload\\");
                    }

                    var supportedTypes = new[] { ".jpg", ".jpeg", ".pdf",".png" };
                    var fileExt = "." + System.IO.Path.GetExtension(objFile.files.FileName).Substring(1);
                    if (!supportedTypes.Contains(fileExt))
                    {
                        throw new ArgumentException("File Extension Is InValid - Only Upload jpg/jpeg/pdf/png File");
                    }

                    var fileName = DateTime.Now.Ticks + fileExt;

                    using (FileStream fileStream = System.IO.File.Create(_environment.WebRootPath + "\\Upload\\" + fileName))
                    {
                        objFile.files.CopyTo(fileStream);
                        fileStream.Flush();

                        var file = new CommonFile()
                        {
                            FilePath = Path.Combine(Directory.GetCurrentDirectory(), _environment.WebRootPath + "\\Upload\\", fileName),
                            FileSize = objFile.files.Length.ToString(),
                            FileType = fileExt
                        };
                        context.CommonFiles.InsertOnSubmit(file);
                        context.SubmitChanges();
                        return new Result()
                        {
                            Message = string.Format("Success"),
                            Status = Result.ResultStatus.success,
                            Data = file.FileId
                        };
                    }
                }
                else
                {
                    throw new ArgumentException("Failed");
                }
            }
        }
    }
}
