using System;
using Portal.Model;

namespace Portal.Domain.Extensions
{
    public static class ObjectiveExtensions
    {
        public static void Validate(this Objective objective)
        {
            if (string.IsNullOrEmpty(objective.Value))
                return;

            switch (objective.DataType)
            {
                case "decimal":
                case "percent":
                case "integer":
                {
                    decimal d;

                    if(!decimal.TryParse(objective.Value, out d))
                        throw new ArgumentException(string.Format("Invalid objective value.  DataType is {0}, Value is {1}", objective.DataType, objective.Value));
                }
                break;
            }
        }

        public static int PercentComplete(this Objective objective)
        {
            if (!objective.AutoTrackingEnabled)
                return objective.PercentComplete;

            switch (objective.DataType)
            {
                case "decimal":
                case "percent":
                case "integer":
                {
                    decimal goal, current, baseline;

                    decimal.TryParse(objective.Value, out goal);
                    decimal.TryParse(objective.CurrentValue, out current);
                    decimal.TryParse(objective.BaselineValue, out baseline);

                    if (current >= goal)
                        return 100;

                    var numerator = current - baseline;
                    var denominator = goal - baseline;

                    if (denominator == 0)
                        denominator = 1;

                    return Convert.ToInt32((numerator/denominator) * 100);
                }
            }

            return 0;
        }

        public static string FormattedValue(this Objective objective)
        {
            return FormatValue(objective.Value, objective.DataType);
        }

        public static string FormattedBaselineValue(this Objective objective)
        {
            return FormatValue(objective.BaselineValue, objective.DataType);
        }

        public static string FormattedCurrentValue(this Objective objective)
        {
            return FormatValue(objective.CurrentValue, objective.DataType);
        }

        private static string FormatValue(string value, string dataType)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            switch (dataType)
            {
                case "decimal":
                    {
                        decimal d;

                        if (decimal.TryParse(value, out d))
                            value = d.ToString("C2");
                    }
                    break;
                case "integer":
                    {
                        float i;

                        if (float.TryParse(value, out i))
                            value = i.ToString("N0");
                    }
                    break;
                case "percent":
                    {
                        decimal d;

                        if (decimal.TryParse(value, out d))
                            value = d.ToString("#,##0.00 '%'");
                    }
                    break;
            }

            return value;
        }
    }
}
