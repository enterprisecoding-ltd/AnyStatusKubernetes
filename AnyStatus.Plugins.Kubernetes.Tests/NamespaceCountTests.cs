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
using System;
using System.Collections.Generic;
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
            var widget = new NamespaceCountWidget { ApiUris = new List<string>() { "https://127.0.0.1:6443" } };

            var namespacesResponseMock = new Mock<NamespacesResponse>();
            var kubernetesHelperMock = new Mock<KubernetesHelper>();
            var kubernetesSimpleClientMock = new Mock<KubernetesSimpleClient>(MockBehavior.Strict, new object[] { widget });

            namespacesResponseMock.Setup(response => response.Items).Returns(new ItemEntry[50]);
            namespacesResponseMock.Setup(response => response.IsValid).Returns(true);

            kubernetesHelperMock.Setup(helper => helper.GetKubernetesClient(It.IsAny<IKubernetesWidget>()))
                .Returns(kubernetesSimpleClientMock.Object);

            kubernetesSimpleClientMock.Setup(client => client.NamespacesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(namespacesResponseMock.Object));

            var request = MetricQueryRequest.Create(widget);

            var handler = new NamespaceCountHandler(kubernetesHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Ok, widget.State);
            Assert.AreEqual((long)50, widget.Value);

            kubernetesHelperMock.Verify(client => client.GetKubernetesClient(It.IsAny<IKubernetesWidget>()), Times.Once());
            kubernetesSimpleClientMock.Verify(client => client.NamespacesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestMethod]
        public async Task NamespaceCountShouldInvalidWhenResponseIsInvalid()
        {
            var widget = new NamespaceCountWidget { ApiUris = new List<string>() { "https://127.0.0.1:6443" } };

            var namespacesResponseMock = new Mock<NamespacesResponse>();
            var kubernetesHelperMock = new Mock<KubernetesHelper>();
            var kubernetesSimpleClientMock = new Mock<KubernetesSimpleClient>(MockBehavior.Strict, new object[] { widget });

            namespacesResponseMock.Setup(response => response.IsValid).Returns(false);

            kubernetesHelperMock.Setup(helper => helper.GetKubernetesClient(It.IsAny<IKubernetesWidget>()))
                .Returns(kubernetesSimpleClientMock.Object);

            kubernetesSimpleClientMock.Setup(client => client.NamespacesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(namespacesResponseMock.Object));

            var request = MetricQueryRequest.Create(widget);

            var handler = new NamespaceCountHandler(kubernetesHelperMock.Object);

            await handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            Assert.AreEqual(State.Invalid, widget.State);

            kubernetesHelperMock.Verify(client => client.GetKubernetesClient(It.IsAny<IKubernetesWidget>()), Times.Once());
            kubernetesSimpleClientMock.Verify(client => client.NamespacesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
