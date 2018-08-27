using libMatrix.Backends;
using System;
using System.Diagnostics;

namespace libMatrix
{
    public class MatrixException : Exception
    {
        public MatrixException(string message) : base(message)
        {
            Debug.WriteLine("Matrix Exception thrown: " + message);
        }
        public MatrixException(string message, Exception innerException) : base(message, innerException) {
            Debug.WriteLine("Matrix Exception thrown: " + message + " - " + innerException);
        }
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
