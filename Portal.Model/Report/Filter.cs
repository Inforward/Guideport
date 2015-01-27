using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Portal.Model.Report
{
    public class Filter
    {
        public Filter()
        {
            Options = new List<FilterOption>();
        }

        public int FilterID { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public bool IsRequired { get; set;}
        public string DataTypeName { get; set; }
        public string ParameterName { get; set; }
        public string InputType { get; set; }

        [IgnoreDataMember]
        public ICollection<View> Views { get; set; }

        [NotMapped]
        public object Value { get; set; }

        [NotMapped]
        public object DefaultValue { get; set; }

        [NotMapped]
        public List<FilterOption> Options { get; set; }
    }

    public class FilterOption
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public enum Filters
    {
        FirstName = 1,
        LastName = 2,
        Group = 3,
        BrokerDealer = 4,
        IncludeTerminated = 5,
        ExcludeAdvisorsWithNoData = 6,
        StartDate = 7,
        EndDate = 8,
        Year = 9
    }

    public struct ReportDataTypeNames
    {
        public const string String = "System.String";
        public const string DateTime = "System.DateTime";
        public const string Boolean = "System.Boolean";
        public const string Integer = "System.Int32";
        public const string Decimal = "System.Decimal";
    }

    public static class FilterExtensions
    {
        public static bool HasValidValue(this Filter filter)
        {
            if (filter.Value is string)
                return !string.IsNullOrWhiteSpace(filter.Value.ToString());

            return filter.Value != null && filter.Value.GetType() == Type.GetType(filter.DataTypeName);
        }
    }
}