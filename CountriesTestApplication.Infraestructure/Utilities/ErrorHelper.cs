using RestSharp;
using System.Net;

namespace CountriesTestApplication.Infraestructure.Utilities
{
    public static class ApiErrorHelper
    {
        public static void HandleErrorResponse(RestResponse response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new Exception("Bad request. The request was invalid.");
                case HttpStatusCode.Unauthorized:
                    throw new Exception("Unauthorized. Please check your authentication details.");
                case HttpStatusCode.Forbidden:
                    throw new Exception("Forbidden. You do not have permission to access this resource.");
                case HttpStatusCode.NotFound:
                    throw new Exception("Not found. The requested resource could not be found.");
                case HttpStatusCode.InternalServerError:
                    throw new Exception("Internal server error. Please try again later.");
                default:
                    throw new Exception("An error occurred while fetching data: " + response.StatusDescription);
            }
        }
    }
}
