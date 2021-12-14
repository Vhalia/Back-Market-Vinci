using Back_Market_Vinci.Domaine;
using Back_Market_Vinci.Domaine.Exceptions;
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

            if (exception is UnauthorizedAccessException 
                || exception is UnauthorizedException) code = 401;
            if (exception is ArgumentException 
                || exception is MissingMandatoryInformationException) code = 400;
            if (exception is UserNotFoundException
                || exception is ProductNotFoundException) code = 404;

            Console.WriteLine("ICI");

            Response.StatusCode = code;

            return new ErrorResponse(exception);
        }
    }
}
