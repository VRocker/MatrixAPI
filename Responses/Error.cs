using System.Runtime.Serialization;

namespace libMatrix.Responses
{
    [DataContract]
    public class Error
    {
        [DataMember(Name = "errcode")]
        public string ErrorCode { get; set; }
        [DataMember(Name = "error")]
        public string ErrorMsg { get; set; }
    }

}
