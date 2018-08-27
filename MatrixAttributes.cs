using System;

namespace libMatrix
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MatrixSpec : Attribute
    {
        const string MATRIX_SPEC_URL = "https://matrix.org/docs/spec/";
        public readonly string Url;
        public MatrixSpec(string url)
        {
            Url = MATRIX_SPEC_URL + url;
        }

        public override string ToString()
        {
            return Url;
        }
    }
}
