﻿/*
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
using System.Threading;
using System.Threading.Tasks;
using AnyStatus.API;
using AnyStatus.Plugins.Kubernetes.Shared;

namespace AnyStatus.Plugins.Kubernetes.NamespaceCount
{
    public class NamespaceCountHandler : IRequestHandler<MetricQueryRequest<NamespaceCountWidget>>
    {
        /// <summary>
        /// Kubernetes Helper to retrieve Kubernetes client
        /// </summary>
        private readonly KubernetesHelper kubernetesHelper;

        public NamespaceCountHandler() : this(new KubernetesHelper()) { }

        /// <summary>
        /// Constructer used by unit tests
        /// </summary>
        /// <param name="kubernetesHelper">Kubernetes Helper class instance to use</param>
        internal NamespaceCountHandler(KubernetesHelper kubernetesHelper)
        {
            this.kubernetesHelper = kubernetesHelper;
        }

        public async Task Handle(MetricQueryRequest<NamespaceCountWidget> request, CancellationToken cancellationToken)
        {
            var namespaceCountWidget = request.DataContext;

            var client = kubernetesHelper.GetKubernetesClient(namespaceCountWidget);

            var namespacesResponse = await client.NamespacesAsync(cancellationToken);

            if (namespacesResponse.IsValid)
            {
                request.DataContext.Value = namespacesResponse.Items.Length;
                request.DataContext.State = State.Ok;
            }
            else
            {
                namespaceCountWidget.State = State.Invalid;
            }
        }
    }
}