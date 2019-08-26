using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace AnyStatus.Plugins.Kubernetes.Shared
{
    public class AuthenticationMethodsItemsSource : IItemsSource
    {
        /// <summary>
        /// Maps <see cref="AuthenticationMethods"/> entries wit human redable texts
        /// </summary>
        /// <returns>Returns mapped entries</returns>
        public ItemCollection GetValues()
        {
            ItemCollection values = new ItemCollection
            {
                { AuthenticationMethods.HTTPBasicAuthentication, "HTTP Basic Authentication" },
                { AuthenticationMethods.OAuth2, "OAuth2" },
            };

            return values;
        }
    }
}
