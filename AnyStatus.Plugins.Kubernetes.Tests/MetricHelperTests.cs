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
