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
using AnyStatus.Plugins.Kubernetes.Shared;

namespace AnyStatus.Plugins.Kubernetes.PodCount
{
    public class PodCountHandler : IRequestHandler<MetricQueryRequest<PodCountWidget>>
    {
        /// <summary>
        /// Kubernetes Helper to retrieve Kubernetes client
        /// </summary>
        private readonly KubernetesHelper kubernetesHelper;

        public PodCountHandler() : this(new KubernetesHelper()) { }

        /// <summary>
        /// Constructer used by unit tests
        /// </summary>
        /// <param name="kubernetesHelper">Kubernetes Helper class instance to use</param>
        internal PodCountHandler(KubernetesHelper kubernetesHelper)
        {
            this.kubernetesHelper = kubernetesHelper;
        }

        public async Task Handle(MetricQueryRequest<PodCountWidget> request, CancellationToken cancellationToken)
        {
            var podCountWidget = request.DataContext;

            var client = kubernetesHelper.GetKubernetesClient(podCountWidget);

            var podsResponse = await client.GetPodsAsync(podCountWidget.Namespace, cancellationToken);

            if (podsResponse.IsValid)
            {
                request.DataContext.Value = podsResponse.Items.Length;
                request.DataContext.State = State.Ok;
            }
            else
            {
                podCountWidget.State = State.Invalid;
            }
        }
    }
}
