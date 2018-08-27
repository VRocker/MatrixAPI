using System.Runtime.Serialization;

namespace libMatrix.Requests.UserData
{
    [DataContract]
    public class UserProfileSetDisplayName
    {
        [DataMember(Name = "displayname")]
        public string DisplayName { get; set; }
    }
}
