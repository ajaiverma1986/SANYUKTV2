

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
        SIMPLE_PASSWORD=107,
        INVALID_PARAMETERS = 108,
        //Available Limit Detail
        INSUFFICIENT_LIMIT = 109,
        //Transaction Error
        INVALID_TRANSACTION_AMOUNT = 110,
        INVALID_SERVICE_MAPPING = 111,
        PAYMENT_NOT_MATCH = 112,
        //Export ToExcel Error
        NO_RECORD_FOUND = 113,
        UNEXPECTED_ERROR_FOUND = 114,
        BAD_REQUEST = 115,
        //MMTC related
        APPLICATION_ERROR = 116,
        AUTHORIZATION_FAILED = 117,
        BADREQUEST_METHOD_ERROR = 118,
        METHOD_NOT_FOUND = 119
    }
}
