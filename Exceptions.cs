using libMatrix.Backends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libMatrix
{
    public class MatrixException : Exception
    {
        public MatrixException(string message) : base(message) { }
        public MatrixException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class MatrixServerError : MatrixException
    {
        public readonly MatrixErrorCode ErrorCode;
        public readonly string ErrorCodeStr;

        public MatrixServerError(string errorCode, string message) : base(message)
        {
            if (!Enum.TryParse(errorCode, out ErrorCode))
                ErrorCode = MatrixErrorCode.CL_UNKNOWN_ERROR_CODE;

            ErrorCodeStr = errorCode;
        }
    }
}
