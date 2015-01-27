using System;

namespace Portal.Web.PdfGenerators
{
    public static class PdfGeneratorFactory
    {
        public static IPdfGenerator CreateGenerator(PdfGeneratorTypes type)
        {
            IPdfGenerator pdfGenerator;

            switch (type)
            {
                case PdfGeneratorTypes.Survey:
                    pdfGenerator = new SurveyPdfGenerator();
                    break;
                case PdfGeneratorTypes.BusinessAssessmentSurvey:
                    pdfGenerator = new SurveyPdfGenerator();
                    break;
                case PdfGeneratorTypes.BusinessPlan:
                    pdfGenerator = new BusinessPlanPdfGenerator();
                    break;
                default:
                    throw new ArgumentException("Invalid PDF Generator Type");
            }

            return pdfGenerator;
        }
    }

    public enum PdfGeneratorTypes
    {
        Survey,
        BusinessAssessmentSurvey,
        BusinessPlan
    }
}