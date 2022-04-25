using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Common.File
{
    public class Uploads
    {
        private readonly IWebHostEnvironment _environment;

        public Uploads(IWebHostEnvironment Environment)
        {
            _environment = Environment;
        }

        public Result Upload(Models.Common.File.Upload objFile)
        {
            using (DBContext c= new DBContext())
            {
                if (objFile.files == null) 
                {
                    throw new ArgumentException("No file found!");
                }
                if (objFile.files.Length > 0)
                {
                    if (!Directory.Exists(_environment.WebRootPath + "/Upload/"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "/Upload/");
                    }
                    var supportedTypes = new[] { ".jpg", ".jpeg", ".pdf",".png" };
                    var fileExt = "." + System.IO.Path.GetExtension(objFile.files.FileName).Substring(1);
                    if (!supportedTypes.Contains(fileExt))
                    {
                        throw new ArgumentException("File Extension Is InValid - Only Upload jpg/jpeg/pdf/png File");
                    }

                    var fileName = DateTime.Now.Ticks + fileExt; 
                    
                    using (FileStream fileStream = System.IO.File.Create(_environment.WebRootPath + "/Upload/" + fileName))
                    {
                        objFile.files.CopyTo(fileStream);
                        fileStream.Flush();
                        var FGUID = Guid.NewGuid();
                        var file = new CommonFile()
                        {
                            FGUID = FGUID.ToString(),
                            FilePath = Path.Combine(Directory.GetCurrentDirectory(), _environment.WebRootPath + "/Upload/",fileName),
                            FileSize = objFile.files.Length.ToString(),
                            FileType = fileExt,
                        };
                        c.CommonFiles.InsertOnSubmit(file);
                        c.SubmitChanges();
                        return new Result()
                        {
                            Message = "File uploaded successfully!",
                            Status = Result.ResultStatus.success,
                            Data = FGUID,
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
