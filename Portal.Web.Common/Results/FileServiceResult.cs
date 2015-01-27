using System.Web.Mvc;
using Portal.Infrastructure.Helpers;
using Portal.Model;

namespace Portal.Web.Common.Results
{
    public class FileServiceResult : ActionResult
    {
        public File File { get; set; }

        public FileServiceResult(File file)
        {
            File = file;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            var file = File;
            var contentType = file.Info.Extension.TrimStart('.').MapFromCodeTo(ContentDocumentType.Pinion).GetMappedSecondCode();

            response.BufferOutput = false;
            response.ContentType = contentType ?? "application/octet-stream";
            response.AddHeader("Content-Disposition", string.Format("inline; filename=\"{0}\"", file.Info.Name));
            response.AppendHeader("content-length", file.Length.ToString());

            var buffer = new byte[FileHelper.DefaultBufferSize];
            var bytesRead = file.Stream.Read(buffer, 0, buffer.Length);

            while (bytesRead > 0)
            {
                response.OutputStream.Write(buffer, 0, bytesRead);
                response.Flush();
                bytesRead = file.Stream.Read(buffer, 0, buffer.Length);
            }
        }
    }
}
