using Portal.Model;
using Portal.Services.Contracts;

namespace Portal.Web.PdfGenerators
{
    public interface IPdfGenerator
    {
        string Url { get; set; }
        string Title { get; set; }
        User User { get; set; }
        IFileService FileService { get; set; }

        byte[] GeneratePdf();
    }
}
