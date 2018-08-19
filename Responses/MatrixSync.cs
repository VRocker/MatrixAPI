using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace libMatrix.Responses
{
    [DataContract]
    public class MatrixSync
    {
        [DataMember(Name = "next_batch")]
        public string NextBatch { get; set; }
        [DataMember(Name = "account_data")]
        public MatrixSyncEvents AccountData { get; set; }
        [DataMember(Name = "presence")]
        public MatrixSyncEvents Presense { get; set; }
    }

    [DataContract]
    public class MatrixSyncEvents
    {

    }
}
