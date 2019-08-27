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
