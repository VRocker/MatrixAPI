using System.Runtime.Serialization;

namespace libMatrix.Responses.Events.Room
{
    [DataContract]
    public class Member : MatrixEvents
    {
        [DataMember(Name = "content")]
        public MemberContent Content { get; set; }
    }

    [DataContract]
    public class MemberContent
    {
        [DataMember(Name = "avatar_url")]
        public string AvatarUrl { get; set; }
        [DataMember(Name = "displayname")]
        public string DisplayName { get; set; }
        [DataMember(Name = "membership")]
        public string Membership { get; set; }

        // TODO: Third party invite
    }
}
