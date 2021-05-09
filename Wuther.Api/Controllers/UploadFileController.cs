using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Wuther.Util.Helper;
using Wuther.Util.Models;

namespace Wuther.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadFileController : BaseController
    {
        public UploadFileController()
        {

        }
        [HttpGet]
        public IActionResult GetFile()
        {
            return new JsonResult("成功");
        }

        [HttpPost(Name = nameof(UploadPicture))]
        public IActionResult UploadPicture()
        {
            var files = HttpContext.Request.Form.Files;
            ImgResult result = new ImgResult();
            IList<ImgInfo> imgInfos = new List<ImgInfo>();
            foreach (IFormFile file in files)
            {
                ImgInfo img = new ImgInfo();
                img.Url = $"\\admin\\img\\{file.FileName}";
                img.alt = "";
                img.href = $"\\admin\\img\\{file.FileName}";
                //判断是否有重复图片
                using (FileStream stream = System.IO.File.Create($"D:\\Project4\\wuther.ui.vue3\\public\\admin\\img\\{file.FileName}"))
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    var fileName = file.FileName;
                    stream.Flush();
                }
                imgInfos.Add(img);
            }
            result.Errno = 0;
            result.Data = imgInfos;
            return new JsonResult(result);
        }
    }
}
