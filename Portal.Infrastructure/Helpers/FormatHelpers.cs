using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Portal.Infrastructure.Helpers
{
    public static class FormatHelpers
    {
        static readonly Regex digitsOnly = new Regex(@"[^\d]");   

        public static string Capitalize(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            return input.Substring(0, 1).ToUpper() + input.Substring(1);
        }

        public static string FormatAsPhoneNo(this string phoneNumber, bool includeParentheses = true, string emptyText = "---")
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return emptyText;

            var result = phoneNumber.Trim();
            var ext = string.Empty;

            if (result.IndexOf("/") != -1)
            {
                ext = result.Substring(result.IndexOf("/") + 1);
                result = result.Substring(0, result.IndexOf("/"));
            }

            if (result.Length == 10)
            {
                result = string.Format(includeParentheses ? "({0}) {1}-{2}" : "{0}-{1}-{2}", result.Substring(0, 3), result.Substring(3, 3), result.Substring(6));

                if (!string.IsNullOrEmpty(ext))
                    result += " ext: " + ext;
            }

            return result;
        }

        public static string FormatAsPhoneNoWithExtension(this string phoneNumber, string extension, string emptyText = "---")
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return emptyText;

            var result = phoneNumber.Trim();
            var ext = extension;

            if (result.IndexOf("/") != -1 && string.IsNullOrEmpty(ext))
            {
                ext = result.Substring(result.IndexOf("/") + 1);
                result = result.Substring(0, result.IndexOf("/"));
            }

            result = digitsOnly.Replace(result, string.Empty);

            if (result.Length == 10)
            {
                result = string.Format("({0}) {1}-{2}", result.Substring(0, 3), result.Substring(3, 3), result.Substring(6));

                if (!string.IsNullOrEmpty(ext))
                    result += " ext: " + ext;
            }

            return result;
        }

        public static string FormatXml(this string xmlString)
        {
            var xmlDoc = new XmlDocument();
            var sb = new StringBuilder();
            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            };

            xmlDoc.LoadXml(xmlString);

            using (var writer = XmlWriter.Create(sb, settings))
            {
                xmlDoc.Save(writer);
            }

            return sb.ToString();            
        }
    }
}
