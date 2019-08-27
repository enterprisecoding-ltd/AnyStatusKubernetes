using System.Text.RegularExpressions;

namespace AnyStatus.Plugins.Kubernetes.Shared
{
    internal static class MetricHelper
    {
        private const string UNIT_FORMAT = "(?<value>\\d+)(?<unit>[a-zA-Z]+)";

        public static ValueUnit ParseCPUUnit(string input)
        {
            var regex = new Regex(UNIT_FORMAT);
            var matches = regex.Match(input);

            var unit = matches.Groups["unit"].Value;

            var result = new ValueUnit
            {
                Value = double.Parse(matches.Groups["value"].Value),
                Unit = unit == "n" ? "Nanocores" : "Millicores"
            };

            if (unit == "m")
            {
                result.BaseValue = result.Value * 10;
            }
            else
            {
                result.BaseValue = result.Value;
            }

            return result;
        }

        public static ValueUnit ParseMemoryUnit(string input)
        {
            var regex = new Regex(UNIT_FORMAT);
            var matches = regex.Match(input);

            var result = new ValueUnit
            {
                Value = double.Parse(matches.Groups["value"].Value)
            };

            result.Unit = matches.Groups["unit"].Value;

            switch (result.Unit)
            {
                case "Ki":
                    result.BaseValue = result.Value * 1024;
                    break;
                case "Mi":
                    result.BaseValue = result.Value * 1024 * 1024;
                    break;
                case "Gi":
                    result.BaseValue = result.Value * 1024 * 1024 * 1024;
                    break;
                case "Ti":
                    result.BaseValue = result.Value * 1024 * 1024 * 1024 * 1024;
                    break;
            }

            return result;
        }
    }
}
