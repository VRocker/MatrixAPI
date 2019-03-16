using System.Runtime.Serialization;

namespace libMatrix.Requests.Rooms.Message
{
    [DataContract]
    public class MatrixRoomMessageLocation : MatrixRoomMessageBase
    {
        public MatrixRoomMessageLocation()
            : base()
        {
            base.MessageType = "m.location";
        }

        [DataMember(Name = "body", IsRequired = true)]
        public string Description { get; set; }

        [DataMember(Name = "geo_uri", IsRequired = true)]
        public string GeoUri { get; set; }

        [DataMember(Name = "thumbnail_url", IsRequired = false)]
        public string ThumbnailUrl { get; set; }

        [DataMember(Name = "thumbnail_info", IsRequired = false)]
        public APITypes.MatrixContentImageInfo ThumbnailInfo { get; set; }
    }
}
