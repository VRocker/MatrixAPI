using System.Runtime.Serialization;

namespace libMatrix.Responses.Events
{
    [DataContract]
    public class Presence : MatrixEvents
    {
        [DataMember(Name = "content")]
        public PresenceContent Content { get; set; }
    }

    [DataContract]
    public class PresenceContent
    {
        [DataMember(Name = "currently_active")]
        public bool CurrentlyActive { get; set; }
        [DataMember(Name = "last_active_ago")]
        public long LastActiveAgo { get; set; }
        [DataMember(Name = "presence")]
        public string Presence { get; set; }
    }
}
