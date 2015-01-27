using System;
using System.Drawing;
using System.Web.Hosting;
using EvoPdf;

namespace Portal.Web.PdfGenerators
{
    public class SurveyPdfGenerator : BasePdfGenerator
    {
        public override byte[] GeneratePdf()
        {
            Initialize();

            var pdfBytes = base.GeneratePdf();

            return pdfBytes;
        }

        protected override void Initialize()
        {
            base.Initialize();

            PdfConverter.PdfDocumentOptions.LiveUrlsEnabled = true;
            PdfConverter.PdfDocumentOptions.InternalLinksEnabled = true;

            AddHeader();
            AddFooter();
        }

        private void AddHeader()
        {
            PdfConverter.PdfDocumentOptions.ShowHeader = true;
            PdfConverter.PdfHeaderOptions.HeaderHeight = 38;

            //PdfConverter.PdfDocumentOptions.Y = 5;
            //PdfConverter.PdfDocumentOptions.TopSpacing = 10;

            var headerWidth = PdfConverter.PdfDocumentOptions.PdfPageSize.Width - PdfConverter.PdfDocumentOptions.LeftMargin - PdfConverter.PdfDocumentOptions.RightMargin; 

            // Add line element to the bottom of the header
            PdfConverter.PdfHeaderOptions.AddElement(new LineElement(0, 37, headerWidth, 37)
            {
                ForeColor = Color.LightGray
            });

            // Logo
            var logo = new ImageElement(0, 0, 150, 28, HostingEnvironment.MapPath("~/Assets/Images") + "\\pentameter-logo-large.png");
            PdfConverter.PdfHeaderOptions.AddElement(logo);
        }

        private void AddFooter()
        {
            PdfConverter.PdfDocumentOptions.ShowFooter = true;
            PdfConverter.PdfFooterOptions.FooterHeight = 40;

            // Draw footer line
            var footerWidth = PdfConverter.PdfDocumentOptions.PdfPageSize.Width - PdfConverter.PdfDocumentOptions.LeftMargin - PdfConverter.PdfDocumentOptions.RightMargin;
            PdfConverter.PdfFooterOptions.AddElement(new LineElement(0, 0, footerWidth, 0) { ForeColor = Color.Gray });

            // Title
            PdfConverter.PdfFooterOptions.AddElement(new TextElement(0, 25, Title, new Font(new FontFamily("Tahoma"), 11, GraphicsUnit.Pixel))
            {
                EmbedSysFont = true,
                TextAlign = HorizontalTextAlign.Left,
                ForeColor = Color.Gray
            });

            // Page Numbering
            PdfConverter.PdfFooterOptions.AddElement(new TextElement(0, 25, "Page &p; of &P; ", new Font(new FontFamily("Tahoma"), 11, GraphicsUnit.Pixel))
            {
                EmbedSysFont = true,
                TextAlign = HorizontalTextAlign.Center,
                ForeColor = Color.Gray
            });

            // Date Created
            PdfConverter.PdfFooterOptions.AddElement(new TextElement(0, 25, string.Format("Generated: {0:MM/dd/yyyy}", DateTime.Today), new Font(new FontFamily("Tahoma"), 11, GraphicsUnit.Pixel))
            {
                EmbedSysFont = true,
                TextAlign = HorizontalTextAlign.Right,
                ForeColor = Color.Gray
            });
        }
    }
}