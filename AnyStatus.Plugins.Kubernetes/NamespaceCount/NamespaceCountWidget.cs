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
using AnyStatus.API;
using AnyStatus.Plugins.Kubernetes.Shared;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace AnyStatus.Plugins.Kubernetes.NamespaceCount
{
    [DisplayName("Namespace Count")]
    [DisplayColumn("Kubernetes")]
    [Description("Number of namespace on Kubernetes Cluster")]
    public class NamespaceCountWidget : Sparkline, IKubernetesWidget, ISchedulable
    {
        /// <summary>
        /// Kubernetes Cluster api uris to connect
        /// </summary>
        [Required]
        [PropertyOrder(10)]
        [Category("Namespace Count")]
        [DisplayName("Api Host")]
        [Description("Kubernetes Cluster API server host")]
        public string Host { get; set; }

        /// <summary>
        /// Method used for autheticate client against Kubernetes Cluster
        /// </summary>
        [Required]
        [PropertyOrder(20)]
        [Category("Namespace Count")]
        [DisplayName("Authentication Metod")]
        [RefreshProperties(RefreshProperties.All)]
        [ItemsSource(typeof(AuthenticationMethodsItemsSource))]
        [Description("Kubernetes Cluster API server authentication method")]
        public AuthenticationMethods AuthenticationMetod {
            get => authenticationMetod;
            set
            {
                authenticationMetod = value;

                OnPropertyChanged();

                SetPropertyVisibility(nameof(AccessToken), authenticationMetod == AuthenticationMethods.OAuth2);
                SetPropertyVisibility(nameof(Username), authenticationMetod == AuthenticationMethods.HTTPBasicAuthentication);
                SetPropertyVisibility(nameof(Password), authenticationMetod == AuthenticationMethods.HTTPBasicAuthentication);
            }
        }

        /// <summary>
        /// Service account token for OAuth2 authentication to connect Kubernetes Cluster
        /// </summary>
        [PropertyOrder(30)]
        [Browsable(true)]
        [Category("Namespace Count")]
        [DisplayName("Access Token")]
        [Description("Kubernetes Cluster API access token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Username for Http basic authentication
        /// </summary>
        [PropertyOrder(40)]
        [Browsable(true)]
        [Category("Namespace Count")]
        [DisplayName("Username")]
        [Description("Username to connect Kubernetes cluster")]
        public string Username { get; set; }

        /// <summary>
        /// Password for Http basic authentication
        /// </summary>
        [PropertyOrder(50)]
        [Browsable(true)]
        [Category("Namespace Count")]
        [DisplayName("Password")]
        [Description("Password to connect Kubernetes cluster")]
        public
        string Password { get; set; }

        [Category("Namespace Count")]
        [PropertyOrder(60)]
        [DisplayName("Trust Certificate")]
        [Description("Always trust server certificate")]
        public bool SkipTlsVerify { get; set; }

        private AuthenticationMethods authenticationMetod;

        public NamespaceCountWidget() {
            Name = "Namespace Count";

            Interval = 1;
            Units = IntervalUnits.Minutes;
            AuthenticationMetod = AuthenticationMethods.OAuth2;
        }
    }
}
