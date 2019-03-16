using System.Runtime.Serialization;

namespace libMatrix.APITypes
{
    [DataContract]
    public class MatrixContentImageInfo
    {
        [DataMember(Name = "w")]
        public int Width { get; set; }
        [DataMember(Name = "h")]
        public int Height { get; set; }
        [DataMember(Name = "size")]
        public int FileSize { get; set; }
        [DataMember(Name = "mimetype")]
        public string MimeType { get; set; }
    }
}
