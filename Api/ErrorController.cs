using Back_Market_Vinci.Domaine;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_Market_Vinci.Api
{

    [ApiController]
    public class ErrorController : ControllerBase
    {

        [Route("error")]
        public ErrorResponse Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context.Error; // Your exception
            var code = 500; // Internal Server Error by default

            if (exception is UnauthorizedAccessException) code = 401; // Unauthorized
            if (exception is ArgumentException) code = 400; //BadRequest
            if (exception is ArgumentNullException) code = 400;


            Response.StatusCode = code; // You can use HttpStatusCode enum instead

            return new ErrorResponse(exception); // Your error model
        }
    }
}
