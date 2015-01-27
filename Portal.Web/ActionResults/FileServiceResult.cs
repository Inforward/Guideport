using System.Web.Mvc;
using Portal.Model;
using Portal.Infrastructure.Helpers;
using Portal.Services.Contracts;

namespace Portal.Web.ActionResults
{
    public class FileServiceResult : ActionResult
    {
        #region Private Members

        private readonly SiteContentViewModel _content;

        #endregion

        #region Constructor

        public FileServiceResult(SiteContentViewModel content)
        {
            _content = content;
        }

        #endregion

        #region Overrides

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;

            var fileService = NinjectWebCommon.Resolve<IFileService>();

            var file = fileService.DownloadFile(new FileRequest { FileID = _content.FileID });

            response.BufferOutput = false;
            response.ContentType = _content.DocumentType.GetMappedSecondCode() ?? "application/octet-stream";
            response.AddHeader("Content-Disposition", string.Format("inline; filename=\"{0}\"", file.Info.Name));
            response.AppendHeader("content-length", file.Length.ToString());

            var buffer = new byte[FileHelper.DefaultBufferSize];
            var bytesRead = file.Stream.Read(buffer, 0, buffer.Length);

            while (bytesRead > 0)
            {
                if (response.IsClientConnected)
                {
                    response.OutputStream.Write(buffer, 0, bytesRead);
                    response.Flush();

                    bytesRead = file.Stream.Read(buffer, 0, buffer.Length);
                }
                else
                {
                    bytesRead = -1;
                }
            }
        }

        #endregion
    }
}