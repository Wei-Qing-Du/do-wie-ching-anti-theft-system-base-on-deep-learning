using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Anti_thief.Controllers
{
    public class CameraControllers : Controller
    {
        private IHostingEnvironment _environment;

        public CameraControllers(IHostingEnvironment hostingEnvironment)
        {
            _environment = hostingEnvironment;
        }

        [HttpPost]
        public IActionResult Capture(string name)
        {
            {
                var files = HttpContext.Request.Form.Files;
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            // Getting Filename  
                            var fileName = file.FileName;
                            // Unique filename "Guid"  
                            var myUniqueFileName = Convert.ToString(Guid.NewGuid());
                            // Getting Extension  
                            var fileExtension = Path.GetExtension(fileName);
                            // Concating filename + fileExtension (unique filename)  
                            var newFileName = string.Concat(myUniqueFileName, fileExtension);
                            //  Generating Path to store photo   
                            var filepath = Path.Combine(this._environment.WebRootPath, "CameraPhotos") + $@"\{newFileName}";

                            if (!string.IsNullOrEmpty(filepath))
                            {
                                // Storing Image in Folder  
                                StoreInFolder(file, filepath);
                            }

                            var imageBytes = System.IO.File.ReadAllBytes(filepath);
                            if (imageBytes != null)
                            {
                                // Storing Image in Folder  
                               // StoreInDatabase(imageBytes);
                            }

                        }
                    }
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
        }

        private void StoreInFolder(IFormFile file, string fileName)
        {
            using (FileStream fs = System.IO.File.Create(fileName))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
        }

        [Route("")]
        [Route("Cmera/CaptureIndex")]
        public IActionResult CaptureIndex()
        {
            return View();
        }
    }
}
