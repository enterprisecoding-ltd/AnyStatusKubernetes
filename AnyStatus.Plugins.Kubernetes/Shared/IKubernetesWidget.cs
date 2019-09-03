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

namespace AnyStatus.Plugins.Kubernetes.Shared
{
    public interface IKubernetesWidget
    {
        /// <summary>
        /// Kubernetes Cluster api server uri to connect
        /// </summary>
        string Host { get; set; }

        /// <summary>
        /// Method used for autheticate client against Kubernetes Cluster
        /// </summary>
        AuthenticationMethods AuthenticationMetod { get; set; }

        /// <summary>
        /// Service account token for OAuth2 authentication to connect Kubernetes Cluster
        /// </summary>
        string AccessToken { get; set; }

        /// <summary>
        /// Username for Http basic authentication
        /// </summary>
        string Username { get; set; }

        /// <summary>
        /// Password for Http basic authentication
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// Should we trust unknown certificates?
        /// </summary>
        bool SkipTlsVerify { get; set; }
    }
}