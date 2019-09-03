/*
Anystatus Kubernetes plugin
Copyright 2019 Fatih Boy

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
 */
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
