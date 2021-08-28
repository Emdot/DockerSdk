using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DockerSdk.Core.Models
{
    [DataContract]
    public class NetworksPruneResponse // (types.NetworksPruneReport)
    {
        [DataMember(Name = "NetworksDeleted", EmitDefaultValue = false)]
        public IList<string> NetworksDeleted { get; set; }
    }
}
