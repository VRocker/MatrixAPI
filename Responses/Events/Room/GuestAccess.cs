using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace libMatrix.Responses.Events.Room
{
    [DataContract]
    public class GuestAccess : MatrixEvents
    {
        [DataMember(Name = "content")]
        public GuestAccessContent Content { get; set; }
    }

    [DataContract]
    public class GuestAccessContent
    {
        [DataMember(Name = "guest_access", IsRequired = true)]
        public string GuestAccess { get; set; }
    }
}
