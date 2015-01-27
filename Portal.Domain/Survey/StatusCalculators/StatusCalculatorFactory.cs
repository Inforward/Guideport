
namespace Portal.Domain.Survey.StatusCalculators
{
    internal class StatusCalculatorFactory
    {
        public static IStatusCalculator CreateCalculator(string statusCalculatorName)
        {
            IStatusCalculator calculator;

            switch (statusCalculatorName)
            {
                case "WeightedAnswer":
                    calculator = new WeightedAnswerCalculator();
                    break;
                default:
                    calculator = new DefaultCalculator();
                    break;
            }

            return calculator;
        }
    }
}
