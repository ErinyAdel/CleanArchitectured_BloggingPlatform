using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Application.Constants
{
    public enum ResponseStatusCode
    {
        Success = 200,
        Created = 201,
        Accepted = 201,
        NoContent = 204,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        MethodNotAllowed = 405,
        NotAcceptable = 406,
        Conflict = 409,
        InternalServerError = 500,

        //NoRequestWithBusinessIdForUser = 1,
        //RequestAlreadySubmitted = 2,
        //ModelNotValid = 3,
        //OTPNotValid = 4,
        //SubmitGrievanceReassessmentNotAllowedPeriod = 5,
        //EGCAPExamResheduleNotAllowed = 6,
        //EGCAPRetestNotAllowed = 7,
        //EGCAPReRequestNotAllowed = 8,
        //NoFacilityRegisteredwithCode = 9,
        //UserNotFound = 10,
        //UserBlocked = 11,
        //UserLockout = 12,
        //LoginFailed = 13,
        //NotAllMembersReadyToSubmit = 14,
        //MedicalMemberErrorInPercentage = 15,
        //BpmResponseFailure = 99,
    }
}
