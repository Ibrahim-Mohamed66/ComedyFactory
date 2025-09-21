using Microsoft.AspNetCore.Mvc;
//using SparkSystems;

namespace Admin.Components
{
    [ViewComponent(Name = "InitFloara")]
    public class InitFloaraViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var script = "<script id='spark-fe'>try{(function (k){localStorage.FEK=k;t=document.getElementById('spark-fe');t.parentNode.removeChild(t);})('fPELe1OMFCIe2LUH1IT==')}catch(e){}</script>";
            return View("Default", script);
        }
    }

    //[Produces("application/json")]
    //[Route("api/FroalaApi/[action]")]
    //public class FroalaApiController : Controller
    //{
    //    private readonly string imagePath = "wwwroot/uploads/editor/images/";
    //    private readonly string filePath = "wwwroot/uploads/editor/files/";


    //    //[ChildActionOnly]

    //    public IActionResult UploadImage()
    //    {
    //        object response;

    //        try
    //        {
    //            response = FroalaEditor.Image.Upload(HttpContext, imagePath);
    //            return Json(response);
    //        }
    //        catch (Exception e)
    //        {
    //            return Json(e);
    //        }
    //    }

    //    public IActionResult UploadFile()
    //    {

    //        object response;
    //        try
    //        {
    //            response = FroalaEditor.File.Upload(HttpContext, filePath);
    //            return Json(response);
    //        }
    //        catch (Exception e)
    //        {
    //            return Json(e);
    //        }
    //    }

    //    public IActionResult LoadImages()
    //    {

    //        try
    //        {
    //            return Json(FroalaEditor.Image.List(imagePath));
    //        }
    //        catch (Exception e)
    //        {
    //            return Json(e);
    //        }
    //    }

    //    public IActionResult UploadImageResize()
    //    {

    //        MagickGeometry resizeGeometry = new MagickGeometry(1024, 0)
    //        {
    //            Greater = true,
    //            IgnoreAspectRatio = false
    //        };

    //        FroalaEditor.ImageOptions options = new FroalaEditor.ImageOptions
    //        {
    //            ResizeGeometry = resizeGeometry
    //        };

    //        try
    //        {
    //            return Json(FroalaEditor.Image.Upload(HttpContext, imagePath, options));
    //        }
    //        catch (Exception e)
    //        {
    //            return Json(e);
    //        }
    //    }

    //    public IActionResult UploadImageValidation()
    //    {

    //        Func<string, string, bool> validationFunction = (path, mimeType) =>
    //        {

    //            MagickImageInfo info = new MagickImageInfo(path);

    //            if (info.Width != info.Height)
    //            {
    //                return false;
    //            }

    //            return true;
    //        };

    //        FroalaEditor.ImageOptions options = new FroalaEditor.ImageOptions
    //        {
    //            Fieldname = "myImage",
    //            Validation = new FroalaEditor.ImageValidation(validationFunction)
    //        };

    //        try
    //        {
    //            return Json(FroalaEditor.Image.Upload(HttpContext, imagePath, options));
    //        }
    //        catch (Exception e)
    //        {
    //            return Json(e);
    //        }
    //    }

    //    public IActionResult UploadFileValidation()
    //    {

    //        //Func<string, string, bool> validationFunction = (filePath, mimeType) => {

    //        //    long size = new System.IO.FileInfo(filePath).Length;
    //        //    if (size > 10 * 1024 * 1024)
    //        //    {
    //        //        return false;
    //        //    }

    //        //    return true;
    //        //};

    //        FroalaEditor.FileOptions options = new FroalaEditor.FileOptions
    //        {
    //            // Validation = new FroalaEditor.FileValidation(validationFunction)
    //            Validation = new FroalaEditor.FileValidation(new string[] { "txt", "doc", "docx", "xls", "xlsx", "ppt", "pptx", "csv", "pdf", "zip", "rar" },
    //                             new string[] { "text/plain", "application/msword","application/vnd.openxmlformats-officedocument.wordprocessingml.document",
    //                                 "application/msexcel", "application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
    //                                 "application/vnd.ms-powerpoint","application/vnd.openxmlformats-officedocument.presentationml.presentation" ,
    //                                 "text/csv", "application/x-pdf", "application/pdf" , "application/x-rar-compressed","audio/mpeg","audio/x-mpeg-3","audio/wav","audio/x-wav",
    //                                 "application/zip", "application/x-zip", "application/x-zip-compressed", "application/octet-stream", "application/x-compress",
    //                                 "application/x-compressed", "multipart/x-zip"
    //                             })
    //        };

    //        try
    //        {
    //            return Json(FroalaEditor.Image.Upload(HttpContext, filePath, options));
    //        }
    //        catch (Exception e)
    //        {
    //            return Json(e);
    //        }
    //    }

    //    public IActionResult DeleteFile()
    //    {
    //        try
    //        {
    //            FroalaEditor.File.Delete(HttpContext.Request.Form["src"]);
    //            return Json(true);
    //        }
    //        catch (Exception e)
    //        {
    //            return Json(e);
    //        }
    //    }

    //    public IActionResult DeleteImage()
    //    {
    //        try
    //        {
    //            FroalaEditor.Image.Delete(HttpContext.Request.Form["src"]);
    //            return Json(true);
    //        }
    //        catch (Exception e)
    //        {
    //            return Json(e);
    //        }
    //    }

    //    public object S3Signature()
    //    {
    //        FroalaEditor.S3Config config = new FroalaEditor.S3Config
    //        {
    //            Bucket = Environment.GetEnvironmentVariable("AWS_BUCKET"),
    //            Region = Environment.GetEnvironmentVariable("AWS_REGION"),
    //            KeyStart = Environment.GetEnvironmentVariable("AWS_KEY_START"),
    //            Acl = Environment.GetEnvironmentVariable("AWS_ACL"),
    //            AccessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY"),
    //            SecretKey = Environment.GetEnvironmentVariable("AWS_SECRET_KEY")
    //        };

    //        return Json(FroalaEditor.S3.GetHash(config));
    //    }


    //    public IActionResult Error()
    //    {
    //        return View();
    //    }

    //}
}