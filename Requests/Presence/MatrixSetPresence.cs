using System.Runtime.Serialization;

namespace libMatrix.Requests.Presence
{
    [DataContract]
    public class MatrixSetPresence
    {
        [DataMember(Name = "presence", IsRequired = true)]
        public string Presence { get; set; }
        [DataMember(Name = "status_msg", EmitDefaultValue = false, IsRequired = false)]
        public string StatusMessage { get; set; }
    }
}
