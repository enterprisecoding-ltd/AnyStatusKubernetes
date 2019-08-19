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
using AnyStatus.API;
using AnyStatus.Plugins.Kubernetes.Shared;
using System.Collections.Generic;
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
        [DisplayName("Api Uris")]
        [Description("Kubernetes Cluster API server uris")]
        public List<string> ApiUris { get; set; }

        /// <summary>
        /// Service account token to connect Kubernetes Cluster
        /// </summary>
        [Required]
        [PropertyOrder(20)]
        [Category("Namespace Count")]
        [Description("Kubernetes Cluster API access token")]
        public string Token { get; set; }

        [Category("Namespace Count")]
        [PropertyOrder(30)]
        [DisplayName("Trust Certificate")]
        [Description("Always trust server certificate")]
        public bool TrustCertificate { get; set; }
    }
}
