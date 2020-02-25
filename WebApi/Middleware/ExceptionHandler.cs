using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Tymish.Application.Exceptions;

namespace Tymish.WebApi.Middleware
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _request;

        public ExceptionHandler(RequestDelegate pipeline)
        {
            _request = pipeline;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _request(context);
            }
            catch (NotFoundException e)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Response.Headers.Clear();
                
                // Log e
                Console.WriteLine(e.Message);
            }
        }
    }
}