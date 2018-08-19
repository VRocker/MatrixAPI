using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace libMatrix.Requests.Session
{
    [DataContract]
    public class MatrixLoginPassword : MatrixLogin
    {
        public MatrixLoginPassword(string _user, string _pass)
        {
            _type = "m.login.password";

            User = _user;
            Password = _pass;
        }

        [DataMember(Name = "user")]
        public string User { get; set; }
        [DataMember(Name ="password")]
        public string Password { get; set; }
    }
}
