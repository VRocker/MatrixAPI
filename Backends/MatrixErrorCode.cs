namespace libMatrix.Backends
{
    public enum MatrixErrorCode
    {
        M_FORBIDDEN,
        M_UNKNOWN_TOKEN,
        M_BAD_JSON,
        M_NOT_JSON,
        M_NOT_FOUND,
        M_LIMIT_EXCEEDED,
        M_USER_IN_USE,
        M_INVALID_USERNAME,
        M_ROOM_IN_USE,
        M_INVALID_ROOM_STATE,
        M_BAD_PAGINATION,
        M_THREEPID_IN_USE,
        M_THREEPID_NOT_FOUND,
        M_SERVER_NOT_TRUSTED,
        M_EXCLUSIVE,
        M_UNKNOWN,
        M_TOO_LARGE,
        CL_UNKNOWN_ERROR_CODE,
        CL_NONE
    }
}
