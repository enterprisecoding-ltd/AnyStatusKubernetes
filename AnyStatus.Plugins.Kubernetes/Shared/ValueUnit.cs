namespace AnyStatus.Plugins.Kubernetes.Shared
{
    public class ValueUnit
    {
        /// <summary>
        /// Value in base unit (i.e.: in nanocores)
        /// </summary>
        public double BaseValue { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Unit
        /// </summary>
        public string Unit { get; set; }
    }
}
