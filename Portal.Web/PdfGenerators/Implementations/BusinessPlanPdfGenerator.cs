using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using EvoPdf;
using Portal.Domain;
using Portal.Infrastructure.Configuration;
using Portal.Model;

namespace Portal.Web.PdfGenerators
{
    public class BusinessPlanPdfGenerator : BasePdfGenerator
    {
        public override byte[] GeneratePdf()
        {
            Initialize();

            var pdfBytes = base.GeneratePdf();

            ApplyTemplate(ref pdfBytes);

            return pdfBytes;
        }

        protected override void Initialize()
        {
            base.Initialize();

            PdfConverter.ConversionDelay = 2;
            PdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.Letter;

            AddHeader();
        }

        private void AddHeader()
        {
            PdfConverter.PdfDocumentOptions.ShowHeader = true;
            PdfConverter.PdfHeaderOptions.HeaderHeight = 100;

            PdfConverter.PdfHeaderOptions.AddElement(new ImageElement(226, 25, 125, GetLogo(AffiliateLogoTypes.PdfHeader)));
            PdfConverter.PdfHeaderOptions.AddElement(new TextElement(0, 70, string.Format("{0} - {1}", User.DisplayName, Title), new Font(new FontFamily("Tahoma"), 20, GraphicsUnit.Pixel))
                {
                    TextAlign = HorizontalTextAlign.Center,
                    ForeColor = ColorTranslator.FromHtml("#27527E")
                });
        }

        private void ApplyTemplate(ref byte[] pdfBytes)
        {
            var doc = new Document(new MemoryStream(pdfBytes)) { LicenseKey = Settings.EvoPdfLicenseKey };

            // Apply Cover Page
            var coverPage = doc.Pages.InsertNewPage(0, doc.Pages[0].PageSize, doc.Margins, doc.Pages[0].Orientation);

            coverPage.AddElement(new ImageElement(169, 92, 270, GetLogo(AffiliateLogoTypes.PdfCoverSheet)));

            coverPage.AddElement(new TextElement(0, 360, User.DisplayName, doc.AddFont(new Font(new FontFamily("Tahoma"), 40, FontStyle.Bold, GraphicsUnit.Pixel)), new PdfColor(ColorTranslator.FromHtml("#27527E"))) { TextAlign = HorizontalTextAlign.Center });

            coverPage.AddElement(new TextElement(0, 400, Title, doc.AddFont(new Font(new FontFamily("Tahoma"), 30, FontStyle.Regular, GraphicsUnit.Pixel)), new PdfColor(ColorTranslator.FromHtml("#27527E"))) { TextAlign = HorizontalTextAlign.Center });

            coverPage.AddElement(new TextElement(0, 440, string.Format("Created on {0:MM/dd/yyyy}", DateTime.Now), doc.AddFont(new Font(new FontFamily("Tahoma"), 18, GraphicsUnit.Pixel)), new PdfColor(Color.Gray)) { TextAlign = HorizontalTextAlign.Center });

            coverPage.AddElement(new TextElement(0, 670, User.DBAAddress, doc.AddFont(new Font(new FontFamily("Tahoma"), 20, GraphicsUnit.Pixel)), new PdfColor(ColorTranslator.FromHtml("#27527E"))) { TextAlign = HorizontalTextAlign.Center });

            // Apply Page Border
            foreach (PdfPage page in doc.Pages)
            {
                page.AddElement(new RectangleElement(25, 25, page.PageSize.Width - 50, page.PageSize.Height - 50) { ForeColor = new PdfColor(Color.LightGray) });
                page.AddElement(new RectangleElement(27, 27, page.PageSize.Width - 54, page.PageSize.Height - 54) { ForeColor = new PdfColor(Color.LightGray) });

                // Apply Custom Header Line
                if (page.Index > 0)
                    page.AddElement(new LineElement(53, 115, 559, 115) { ForeColor = new PdfColor(Color.LightGray), LineStyle = new LineStyle(0.9f) });
            }

            pdfBytes = doc.Save();
            doc.Close();
        }
    }
}