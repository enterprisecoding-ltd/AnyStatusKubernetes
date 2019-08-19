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
using System.Collections.Generic;

namespace AnyStatus.Plugins.Kubernetes.Shared
{
    public interface IKubernetesWidget
    {
        /// <summary>
        /// Kubernetes Cluster api uris to connect
        /// </summary>
        List<string> ApiUris { get; set; }

        /// <summary>
        /// Service account token to connect Kubernetes Cluster
        /// </summary>
        string Token { get; set; }

        /// <summary>
        /// Should we trust unknown certificates?
        /// </summary>
        bool TrustCertificate { get; set; }
    }
}