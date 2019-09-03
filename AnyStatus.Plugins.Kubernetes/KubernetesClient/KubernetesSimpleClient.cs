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
using AnyStatus.Plugins.Kubernetes.KubernetesClient.Objects;
using AnyStatus.Plugins.Kubernetes.Shared;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Kubernetes.KubernetesClient
{
    /// <summary>
    /// Simple Kubernetes client
    /// </summary>
    /// <remarks>
    /// This class only implements limited Kubernetes client functionality required 
    /// by AnyStatus Kubernetes plugin.
    /// </remarks>
    public class KubernetesSimpleClient
    {
        /// <summary>
        /// List of http clients used to connect Kubernetes cluster members
        /// </summary>
        private readonly HttpClient httpClient;

        public KubernetesSimpleClient(IKubernetesWidget kubernetesWidget)
        {
            httpClient = new HttpClient { BaseAddress = new Uri(kubernetesWidget.Host) };
            if (kubernetesWidget.AuthenticationMetod == AuthenticationMethods.OAuth2)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", kubernetesWidget.AccessToken);
            }
            else if (kubernetesWidget.AuthenticationMetod == AuthenticationMethods.HTTPBasicAuthentication)
            {
                var byteArray = Encoding.ASCII.GetBytes($"{kubernetesWidget.Username}:{kubernetesWidget.Password}");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            }

            if (kubernetesWidget.SkipTlsVerify)
            {
                ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidation;
            }
        }

        /// <summary>
        /// Retrieves Kubernetes Cluster Namespace List
        /// </summary>
        /// <param name="cancellationToken">Token to cancel Kubernetes Cluster request</param>
        /// <returns>Cluster namespace list</returns>
        public virtual async Task<NamespacesResponse> GetNamespacesAsync(CancellationToken cancellationToken)
        {
            NamespacesResponse result;
            try
            {
                HttpResponseMessage responseMessage = await GetAsync("/api/v1/namespaces", cancellationToken);

                var response = await responseMessage.Content.ReadAsStringAsync();

                result = JsonConvert.DeserializeObject<NamespacesResponse>(response);
                result.IsValid = true;
            }
            catch (Exception ex)
            {
                result = new NamespacesResponse { IsValid = false, OriginalException = ex };
            }

            return result;
        }

        /// <summary>
        /// Retrieves Kubernetes Cluster Pod List
        /// </summary>
        /// <param name="namespace">Namespace to list pods</param>
        /// <param name="cancellationToken">Token to cancel Kubernetes Cluster request</param>
        /// <returns>Cluster pod list</returns>
        public virtual async Task<PodsResponse> GetPodsAsync(string @namespace, CancellationToken cancellationToken)
        {
            PodsResponse result;
            try
            {
                var path = string.IsNullOrWhiteSpace(@namespace) ? "api/v1/pods" : $"api/v1/namespaces/{@namespace}/pods";
                HttpResponseMessage responseMessage = await GetAsync(path, cancellationToken);

                var response = await responseMessage.Content.ReadAsStringAsync();

                result = JsonConvert.DeserializeObject<PodsResponse>(response);
                result.IsValid = true;
            }
            catch (Exception ex)
            {
                result = new PodsResponse { IsValid = false, OriginalException = ex };
            }

            return result;
        }

        /// <summary>
        /// Retrieves Kubernetes Cluster Node metrics
        /// </summary>
        /// <param name="nodeName">Node name to retrieve metrics</param>
        /// <param name="cancellationToken">Token to cancel Kubernetes Cluster request</param>
        /// <returns>Node Metrics</returns>
        public virtual async Task<NodeMetricsResponse> GetNodeMetricsAsync(string nodeName, CancellationToken cancellationToken)
        {
            NodeMetricsResponse result;
            try
            {
                HttpResponseMessage responseMessage = await GetAsync($"/apis/metrics.k8s.io/v1beta1/nodes/{nodeName}", cancellationToken);

                var response = await responseMessage.Content.ReadAsStringAsync();

                result = JsonConvert.DeserializeObject<NodeMetricsResponse>(response);
                result.IsValid = true;
            }
            catch (Exception ex)
            {
                result = new NodeMetricsResponse { IsValid = false, OriginalException = ex };
            }

            return result;
        }

        /// <summary>
        /// Retrieves Pod metrics
        /// </summary>
        /// <param name="@namespace">Kubernetes namespace that pod running</param>
        /// <param name="podsName">Pod name to retrieve metrics</param>
        /// <param name="cancellationToken">Token to cancel Kubernetes Cluster request</param>
        /// <returns>Node Metrics</returns>
        public virtual async Task<PodMetricsResponse> GetPodMetricsAsync(string @namespace, string podsName, CancellationToken cancellationToken)
        {
            PodMetricsResponse result;
            try
            {
                HttpResponseMessage responseMessage = await GetAsync($"/apis/metrics.k8s.io/v1beta1/namespaces/{@namespace}/pods/{podsName}", cancellationToken);

                var response = await responseMessage.Content.ReadAsStringAsync();

                result = JsonConvert.DeserializeObject<PodMetricsResponse>(response);
                result.IsValid = true;
            }
            catch (Exception ex)
            {
                result = new PodMetricsResponse { IsValid = false, OriginalException = ex };
            }

            return result;
        }

        private async Task<HttpResponseMessage> GetAsync(string path, CancellationToken cancellationToken)
        {
            var responseMessage = await httpClient.GetAsync(path, cancellationToken);

            responseMessage.EnsureSuccessStatusCode();

            return responseMessage; ;
        }

        private bool RemoteCertificateValidation(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public void Dispose()
        {
            ServicePointManager.ServerCertificateValidationCallback -= RemoteCertificateValidation;

            httpClient.Dispose();
        }
    }
}
