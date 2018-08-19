using System.Runtime.Serialization;

namespace libMatrix.Requests.Session
{
    [DataContract]
    public abstract class MatrixLogin
    {
        [DataMember(Name ="type")]
        public string _type { get; protected set; }

        [DataMember(Name = "device_id", IsRequired = false, EmitDefaultValue = true)]
        public string DeviceID { get; set; }

    }
}
