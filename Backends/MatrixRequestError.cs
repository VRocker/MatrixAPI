using System.Net;

namespace libMatrix.Backends
{
    public class MatrixRequestError
    {
        public readonly string MatrixError;
        public readonly MatrixErrorCode MatrixErrorCode;
        public readonly HttpStatusCode HTTPStatus;
        public bool IsOk {  get { return MatrixErrorCode == MatrixErrorCode.CL_NONE && HTTPStatus == HttpStatusCode.OK; } }

        public MatrixRequestError(string error, MatrixErrorCode code, HttpStatusCode status)
        {
            MatrixError = error;
            MatrixErrorCode = code;
            HTTPStatus = status;
        }

        public string GetErrorString()
        {
            if (HTTPStatus != HttpStatusCode.OK)
                return "Got a HTTP error: " + HTTPStatus + " during request.";
            else if (MatrixErrorCode != MatrixErrorCode.CL_NONE)
                return "Got a Matrix error: " + MatrixError + "[" + MatrixErrorCode + "].";

            return "No Error";
        }

        public override string ToString()
        {
            return GetErrorString();
        }

        public readonly static MatrixRequestError NO_ERROR = new MatrixRequestError("", MatrixErrorCode.CL_NONE, HttpStatusCode.OK);
    }
}
