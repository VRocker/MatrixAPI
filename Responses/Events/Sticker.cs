using System.Runtime.Serialization;

namespace libMatrix.Responses.Events
{
    [DataContract]
    public class Sticker : MatrixEvents
    {
        [DataMember(Name = "content")]
        public StickerContent Content { get; set; }
    }

    [DataContract]
    public class StickerContent
    {
        [DataMember(Name = "body", IsRequired = true)]
        public string Body { get; set; }
        [DataMember(Name = "url", IsRequired = true)]
        public string Url { get; set; }

        // TODO: ImageInfo
    }
}
