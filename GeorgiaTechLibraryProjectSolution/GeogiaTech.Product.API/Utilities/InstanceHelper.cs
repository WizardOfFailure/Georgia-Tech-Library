namespace GeogiaTech.Product.API.Utilities
{
    public class InstanceHelper
    {
        private readonly string instanceId;
        public InstanceHelper()
        {
            //get the instance id from the environment variable
            var instanceIdFromEnv = Environment.GetEnvironmentVariable("INSTANCE_ID");
            // If the environment variable is not set, generate a new Guid
            this.instanceId = !string.IsNullOrEmpty(instanceIdFromEnv) ? instanceIdFromEnv : Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Returns the instanceId
        /// </summary>
        /// <returns></returns>
        public string GetInstanceId()
        {
            return this.instanceId;
        }
    }
}
