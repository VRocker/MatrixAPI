using libMatrix.APITypes;
using System.Runtime.Serialization;

namespace libMatrix.Responses.Events.Room
{
    [DataContract]
    public class Avatar : MatrixEvents
    {
        [DataMember(Name = "content")]
        public AvatarContent Content { get; set; }
    }

    [DataContract]
    public class AvatarContent
    {
        [DataMember(Name = "url")]
        public string Url { get; set; }
        [DataMember(Name = "info")]
        MatrixContentImageInfo Info { get; set; }
        [DataMember(Name = "thumbnail_url")]
        public string ThumbnailUrl { get; set; }
        [DataMember(Name = "thumbnail_info")]
        MatrixContentImageInfo ThumbnailInfo { get; set; }
    }
}
