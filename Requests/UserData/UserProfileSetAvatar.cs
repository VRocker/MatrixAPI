using System.Runtime.Serialization;

namespace libMatrix.Requests.UserData
{
    [DataContract]
    public class UserProfileSetAvatar
    {
        [DataMember(Name = "avatar_url")]
        public string AvatarUrl { get; set; }
    }
}
