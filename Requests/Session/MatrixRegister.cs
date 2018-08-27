using System.Runtime.Serialization;

namespace libMatrix.Requests.Session
{
    [DataContract]
    public class MatrixRegister
    {
        [DataMember(Name = "username", IsRequired = false, EmitDefaultValue = false)]
        public string Username { get; set; }
        [DataMember(Name = "bind_email", IsRequired = false, EmitDefaultValue = false)]
        public bool BindEmail { get; set; }
        [DataMember(Name = "password", IsRequired = true)]
        public string Password { get; set; }
    }
}
