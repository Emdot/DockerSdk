using System.Runtime.Serialization;

namespace DockerSdk.Core.Models
{
    [DataContract]
    public class PluginDescription // (swarm.PluginDescription)
    {
        [DataMember(Name = "Type", EmitDefaultValue = false)]
        public string Type { get; set; }

        [DataMember(Name = "Name", EmitDefaultValue = false)]
        public string Name { get; set; }
    }
}
