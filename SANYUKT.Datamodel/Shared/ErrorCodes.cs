

namespace SANYUKT.Datamodel.Shared
{
    public enum ErrorCodes
    {
        NO_ERROR = 0,
        SERVER_ERROR = 100,
        INCORRECT_PASSWORD = 101,
        VALIDATION_ERROR = 102,
        AUTHENTICATION_FAILURE = 103,
        USER_NOT_ALLOWED_APPLCATION_ACCESS = 104,
        AUTHORIZATION_FAILURE = 105,
        INVALID_API_TOKEN = 106,
        INVALID_UserName=107,
        INVALID_PARAMETERS = 108,
        //Available Limit Detail
        INSUFFICIENT_LIMIT = 109,
        //Transaction Error
        INVALID_TRANSACTION_AMOUNT = 110,
        INVALID_SERVICE_MAPPING = 111,
        //Export ToExcel Error
        NO_RECORD_FOUND = 112,
        UNEXPECTED_ERROR_FOUND = 113,
        BAD_REQUEST = 114,
        //MMTC related
        APPLICATION_ERROR = 115,
        BADREQUEST_METHOD_ERROR = 116,
        METHOD_NOT_FOUND = 117,
        SIMPLE_PASSWORD=118,
        TRANSACTION_NOT_DONE=119
    }
}
