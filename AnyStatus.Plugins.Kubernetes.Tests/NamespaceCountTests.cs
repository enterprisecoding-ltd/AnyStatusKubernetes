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
using System.Threading;
using System.Threading.Tasks;
using AnyStatus.API;
using AnyStatus.Plugins.Kubernetes.KubernetesClient;
using AnyStatus.Plugins.Kubernetes.KubernetesClient.Objects;
using AnyStatus.Plugins.Kubernetes.NamespaceCount;
using AnyStatus.Plugins.Kubernetes.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AnyStatus.Plugins.Kubernetes.Tests
{
    [TestClass]
    public class NamespaceCountTests
    {
        [TestMethod]
        public async Task NamespaceCountShouldValid()
        {
            var widget = new NamespaceCountWidget { Host = "https://127.0.0.1:6443" };

            var namespacesResponseMock = new Mock<NamespacesResponse>();
            var kubernetesHelperMock = new Mock<KubernetesHelper>();
            var kubernetesSimpleClientMock = new Mock<KubernetesSimpleClient>(MockBehavior.Strict, new object[] { widget });

            namespacesResponseMock.Setup(response => response.Items).Returns(new ItemEntry[50]);
            namespacesResponseMock.Setup(response => response.IsValid).Returns(true);

            kubernetesHelperMock.Setup(helper => helper.GetKubernetesClient(It.IsAny<IKubernetesWidget>()))
                .Returns(kubernetesSimpleClientMock.Object);

            kubernetesSimpleClientMock.Setup(client => client.GetNamespacesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(namespacesResponseMock.Object));

            var request = MetricQueryRequest.Create(widget);

            var handler = new NamespaceCountHandler(kubernetesHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Ok, widget.State);
            Assert.AreEqual((long)50, widget.Value);

            kubernetesHelperMock.Verify(client => client.GetKubernetesClient(It.IsAny<IKubernetesWidget>()), Times.Once());
            kubernetesSimpleClientMock.Verify(client => client.GetNamespacesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestMethod]
        public async Task NamespaceCountShouldInvalidWhenResponseIsInvalid()
        {
            var widget = new NamespaceCountWidget { Host = "https://127.0.0.1:6443" };

            var namespacesResponseMock = new Mock<NamespacesResponse>();
            var kubernetesHelperMock = new Mock<KubernetesHelper>();
            var kubernetesSimpleClientMock = new Mock<KubernetesSimpleClient>(MockBehavior.Strict, new object[] { widget });

            namespacesResponseMock.Setup(response => response.IsValid).Returns(false);

            kubernetesHelperMock.Setup(helper => helper.GetKubernetesClient(It.IsAny<IKubernetesWidget>()))
                .Returns(kubernetesSimpleClientMock.Object);

            kubernetesSimpleClientMock.Setup(client => client.GetNamespacesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(namespacesResponseMock.Object));

            var request = MetricQueryRequest.Create(widget);

            var handler = new NamespaceCountHandler(kubernetesHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Invalid, widget.State);

            kubernetesHelperMock.Verify(client => client.GetKubernetesClient(It.IsAny<IKubernetesWidget>()), Times.Once());
            kubernetesSimpleClientMock.Verify(client => client.GetNamespacesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
