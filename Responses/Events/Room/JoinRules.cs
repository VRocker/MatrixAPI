using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace libMatrix.Responses.Events.Room
{
    [DataContract]
    public class JoinRules : MatrixEvents
    {
        [DataMember(Name = "content")]
        public JoinRulesContent Content { get; set; }
    }

    [DataContract]
    public class JoinRulesContent
    {
        [DataMember(Name = "join_rule")]
        public string JoinRule { get; set; }
    }
}
