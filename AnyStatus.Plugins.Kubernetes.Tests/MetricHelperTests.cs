/*
Anystatus Kubernetes plugin
Copyright (C) 2019  Enterprisecoding (Fatih Boy)

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using AnyStatus.Plugins.Kubernetes.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AnyStatus.Plugins.Kubernetes.Tests
{
    [TestClass]
    public class MetricHelperTests
    {
        [TestMethod]
        public void MetricHelperShouldParseNanocores()
        {
            var result = MetricHelper.ParseCPUUnit("62662558n");

            Assert.AreEqual(result.Value, 62662558);
            Assert.AreEqual(result.BaseValue, 62662558);
            Assert.AreEqual(result.Unit, "Nanocores");
        }

        [TestMethod]
        public void MetricHelperShouldParseMillicores()
        {
            var result = MetricHelper.ParseCPUUnit("62662558m");

            Assert.AreEqual(result.Value, 62662558);
            Assert.AreEqual(result.BaseValue, 626625580);
            Assert.AreEqual(result.Unit, "Millicores");
        }

        [TestMethod]
        public void MetricHelperShouldParseKi()
        {
            var result = MetricHelper.ParseMemoryUnit("673584Ki");

            Assert.AreEqual(result.Value, 673584);
            Assert.AreEqual(result.BaseValue, 689750016);
            Assert.AreEqual(result.Unit, "Ki");
        }

        [TestMethod]
        public void MetricHelperShouldParseMi()
        {
            var result = MetricHelper.ParseMemoryUnit("656Mi");

            Assert.AreEqual(result.Value, 656);
            Assert.AreEqual(result.BaseValue, 687865856);
            Assert.AreEqual(result.Unit, "Mi");
        }
    }
}
