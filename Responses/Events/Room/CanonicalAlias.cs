using System.Runtime.Serialization;

namespace libMatrix.Responses.Events.Room
{
    [DataContract]
    public class CanonicalAlias : MatrixEvents
    {
        [DataMember(Name = "content")]
        public AliasContent Content { get; set; }
    }

    [DataContract]
    public class AliasContent
    {
        [DataMember(Name = "alias")]
        public string Alias { get; set; }
    }
}
