using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libMatrix.Backends
{
    public interface IMatrixAPIBackend
    {
        MatrixRequestError Get(string path, bool authenticate, out string result);
        MatrixRequestError Post(string path, bool authenticate, string request, out string result);
        MatrixRequestError Post(string path, bool authenticate, string request, Dictionary<string, string> headers, out string result);
        MatrixRequestError Post(string path, bool authenticate, byte[] request, Dictionary<string, string> headers, out string result);
        MatrixRequestError Put(string path, bool authenticate, string request, out string result);

        void SetAccessToken(string token);
    }
}
