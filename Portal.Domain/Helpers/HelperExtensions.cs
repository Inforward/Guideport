
namespace Portal.Domain.Helpers
{
    public static class HelperExtensions
    {
        public static bool GreaterThanAndLessOrEqualTo(this decimal number, decimal min, decimal max)
        {
            return (number > min && number <= max);
        }

        public static bool GreaterThanAndLessOrEqualTo(this double number, double min, double max)
        {
            return (number > min && number <= max);
        }
    }
}
