using System.Runtime.Serialization;

namespace libMatrix.Responses.Media
{
    [DataContract]
    class MediaUploadResponse
    {
        [DataMember(Name = "content_uri")]
        public string ContentUri { get; set; }
    }
}
