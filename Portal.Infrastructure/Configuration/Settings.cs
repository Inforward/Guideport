using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Portal.Model;
using Portal.Model.App;

namespace Portal.Infrastructure.Configuration
{
    public static class Settings
    {
        public static string EvoPdfLicenseKey
        {
            get { return Get("app:EvoPdf.LicenseKey", string.Empty); }
        }

        public static string UserProfileSchema
        {
            get { return Get("app:UserProfileSchema", string.Empty); }
        }

        public static IEnumerable<ContentDocumentType> SearchableDocumentTypes
        {
            get
            {
                return Get("app:Search.ValidDocumentTypes", string.Empty).Split(',')
                                        .Select(s => s.MapFromCodeTo(ContentDocumentType.Unknown))
                                        .Where(s => s != ContentDocumentType.Unknown);
            }
        }

        public static Environments CurrentEnvironment
        {
            get { return Get("app:Configuration.Environment", Environments.Default); }
        }

        public static bool EnableServices
        {
            get { return Get("app:EnableServices", false); }
        }

        public static T Get<T>(string key, T defaultValue = default(T))
        {
            if (string.IsNullOrWhiteSpace(key))
                return defaultValue;

            string value = ConfigurationManager.AppSettings[key];

            if (value == null)
                return defaultValue;

            Type type = typeof(T);

            if (type.IsEnum)
                return (T)Enum.Parse(type, value, true);

            return (T)Convert.ChangeType(value, type);
        }

        public static class SurveyNames
        {
            public static string Enrollment
            {
                get { return Get("Enrollment.SurveyName", string.Empty); }
            }

            public static string QualifiedBuyer
            {
                get { return Get("QualifiedBuyer.SurveyName", string.Empty); }
            }

            public static string BusinessAssessment
            {
                get { return Get("Pentameter.SurveyName", string.Empty); }
            }
        }
    }
}