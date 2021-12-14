using Back_Market_Vinci.Domaine;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Back_Market_Vinci.Api
{

    [ApiController]
    public class ErrorController : ControllerBase
    {

        [Route("error")]
        public ErrorResponse Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context.Error;
            var code = 500;

            if (exception is UnauthorizedAccessException) code = 401; // Unauthorized
            if (exception is ArgumentException || exception is ArgumentNullException) code = 400; //BadRequest


            Response.StatusCode = code;

            return new ErrorResponse(exception);
        }
    }
}
