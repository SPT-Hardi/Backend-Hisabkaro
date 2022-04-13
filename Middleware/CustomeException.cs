using HisaabKaro.Models.Common;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HisaabKaro.Middleware
{
    public class CustomeException
    {
        private readonly RequestDelegate _next;

        public CustomeException(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            var hasError = false;
            Result result = new Result();
            try
            {
                await _next(context);
            }
            catch (ArgumentException e)
            {
                hasError = true;
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                result = new Result()
                {
                    Message = e.Message,
                    Status = Result.ResultStatus.warning,
                };
            }
            catch (MethodAccessException e)
            {
                hasError = true;
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.NotModified;
                result = new Result()
                {
                    Message = e.Message,
                    Status = Result.ResultStatus.warning,
                };
            }
            catch (UnauthorizedAccessException e)
            {
                hasError = true;
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                result = new Result()
                {
                    Message = e.Message,
                    Status = Result.ResultStatus.warning,
                };
            }
            catch (Exception e)
            {
                hasError = true;
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                result = new Result()
                {
                    Message = e.Message,
                    Status = Result.ResultStatus.warning,
                };
            }
            finally
            {
                if (hasError)
                {
                    var errorJson = JsonConvert.SerializeObject(result);
                    await context.Response.WriteAsync(errorJson);
                }
            }
        }
    }
}
