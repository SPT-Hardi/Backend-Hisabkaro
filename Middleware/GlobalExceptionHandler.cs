using Devart.Data.Linq;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using System.Threading.Tasks;
using HIsabKaro.Models.Common;
using HIsabKaro.Controllers.Filters;

namespace HIsabKaro.Middleware
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var er = new ExceptionResult()
            {
                Request = context.Request.Path
            };

            Result r = null;

            context.Response.ContentType = "application/json";

            var hasError = false;

           // Serilog.Log.Information("Logged");

            try
            {
                await _next.Invoke(context);
            }
            catch (LinqCommandExecutionException ex)
            {
                hasError = true;
                try
                {
                    System.Data.SqlClient.SqlException sqlException = (System.Data.SqlClient.SqlException)ex.InnerException;

                    if (sqlException.Number == 2627 || sqlException.Number == 2601)
                    {
                        r = new Result()
                        {
                            Message = HIsabKaro.Cores.Helpers.StringFunctions.UniqueKeyViolation(sqlException.Message)
                        };
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    }
                    else
                    {
                        r = new Result()
                        {
                            Status = Result.ResultStatus.danger,
                            Message = "An unknown error occurred in database!",
                            Data = new
                            {
                                InnerException = ex.Message,
                                ex.StackTrace
                            }
                        };
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    }
                }
                catch
                {
                    hasError = true;

                    r = new Result()
                    {
                        Status = Result.ResultStatus.danger,
                        Data = ex.StackTrace,
                        Message = ex.Message
                    };
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
            catch (SqlException ex)
            {
                hasError = true;

                if (ex.Number == 2627 | ex.Number == 2601)
                {
                    r = new Result()
                    {
                        Message = HIsabKaro.Cores.Helpers.StringFunctions.UniqueKeyViolation(ex.Message)
                    };
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
                else
                {
                    r = new Result()
                    {
                        Status = Result.ResultStatus.danger,
                        Message = "An unknown error occurred in database!",
                        Data = new
                        {
                            InnerException = ex.Message,
                            ex.StackTrace
                        }
                    };
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
            catch (ArgumentException ex)
            {
                hasError = true;
                var es = ex.Message.Split('|');

                r = new Result()
                {
                    Data = es,
                    Status = Result.ResultStatus.info,
                    Message = "Data validation(s) failed!"
                };
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            catch (HIsabKaro.Controllers.Exceptions.ModelValidationException ex)
            {
                hasError = true;

                r = new Result()
                {
                    Data = ex.ModelState.Values.SelectMany(x => x.Errors).Where(x => string.IsNullOrWhiteSpace(x.ErrorMessage) == false).Select(x => x.ErrorMessage).ToList(),
                    Status = Result.ResultStatus.info,
                    Message = "Data validation(s) failed!"
                };

                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            catch (HttpResponseException ex)
            {
                hasError = true;

                r = new Result()
                {
                    Status = Result.ResultStatus.danger,
                    Message = ex.Value.ToString(),
                    Data = null
                };
                context.Response.StatusCode = ex.Status;
            }
            catch (Exception ex)
            {
                hasError = true;

                r = new Result()
                {
                    Status = Result.ResultStatus.danger,
                    Data = ex.StackTrace,
                    Message = ex.Message
                };
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            finally
            {
                if (hasError)
                {
                    var errorJson = JsonConvert.SerializeObject(r);
                    await context.Response.WriteAsync(errorJson);
                }
            }

        }
    }

    public class ErrorDetails
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }
    }

    internal class ExceptionResult
    {
        public string Request { get; set; }
        public dynamic Content { get; set; }
    }

    internal class ExceptionResultRequest
    {
        public string Path { get; set; }
        public string Method { get; set; }

        public QueryString QueryString { get; set; }

        //public System.IO.Stream Body { get; set; }

        public Microsoft.AspNetCore.Http.IHeaderDictionary Header { get; set; }
    }
}
