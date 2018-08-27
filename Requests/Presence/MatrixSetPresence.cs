using System.Runtime.Serialization;

namespace libMatrix.Requests.Presence
{
    [DataContract]
    public class MatrixSetPresence
    {
        [DataMember(Name = "presence")]
        public string Presence { get; set; }
        [DataMember(Name = "status_msg")]
        public string StatusMessage { get; set; }
    }
}
