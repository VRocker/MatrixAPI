using System.Runtime.Serialization;

namespace libMatrix.Responses
{
    [DataContract]
    public class VersionResponse
    {
        [DataMember(Name = "versions")]
        public string[] Versions { get; set; }
    }

}
