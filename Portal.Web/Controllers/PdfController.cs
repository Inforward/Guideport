using Portal.Infrastructure.Logging;
using Portal.Services.Contracts;
using Portal.Web.PdfGenerators;
using System;
using System.Web.Mvc;

namespace Portal.Web.Controllers
{
    public class PdfController : BaseController
    {
        #region Constructor

        public PdfController(IUserService userService, ILogger logger)
            : base(userService, logger)
        {
        }

        #endregion

        #region Actions

        [HttpGet]
        public ActionResult ExecuteUrl(string url, string title, string type = "Survey")
        {
            PdfGeneratorTypes pdfGeneratorType;

            if (!Enum.TryParse(type, true, out pdfGeneratorType))
                pdfGeneratorType = PdfGeneratorTypes.Survey;

            var pdfGenerator = PdfGeneratorFactory.CreateGenerator(pdfGeneratorType);

            pdfGenerator.Title = title;
            pdfGenerator.User = GetAssistedUser();
            pdfGenerator.Url = url;
            pdfGenerator.FileService = Using<IFileService>();

            var pdfBytes = pdfGenerator.GeneratePdf();

            Response.AddHeader("Content-Type", "application/pdf");
            Response.AddHeader("Content-Disposition", string.Format("inline; filename={1}.pdf; size={0}", pdfBytes.Length, title.Replace(" ", "-")));
            Response.BinaryWrite(pdfBytes);

            return null;
        }

        #endregion
    }
}