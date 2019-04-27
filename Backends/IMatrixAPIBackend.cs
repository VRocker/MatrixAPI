using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace libMatrix.Backends
{
    public interface IMatrixAPIBackend
    {
        Task<Tuple<MatrixRequestError, string>> Get(string path, bool authenticate);
        Task<Tuple<MatrixRequestError, string>> Post(string path, bool authenticate, string request);
        Task<Tuple<MatrixRequestError, string>> Post(string path, bool authenticate, string request, Dictionary<string, string> headers);
        Task<Tuple<MatrixRequestError, string>> Post(string path, bool authenticate, byte[] request, Dictionary<string, string> headers);
        Task<Tuple<MatrixRequestError, string>> Put(string path, bool authenticate, string request);

        Task<Tuple<MatrixRequestError, string>> Delete(string path, bool authenticate);

        void SetAccessToken(string token);
        string GetPath(string apiPath, bool auth);
    }
}
