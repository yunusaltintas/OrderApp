using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OrderApp.Application.Dtos.BaseResponse;
using OrderApp.Application.Dtos.Exceptions;
using Microsoft.IO;
using System.Net;
using System.Text.Json;
using System.Diagnostics;

namespace OrderApp.Application.Middlewares
{
    public class RequestResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseMiddleware> _logger;
        private readonly RecyclableMemoryStreamManager _memoryStreamManager;

        public RequestResponseMiddleware
        (
            RequestDelegate next,
            ILoggerFactory loggerFactory
        )
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<RequestResponseMiddleware>();
            _memoryStreamManager = new RecyclableMemoryStreamManager();
        }

        public async Task Invoke(HttpContext context)
        {
            var watch = new Stopwatch();
            watch.Start();

            string requestMessage = await logRequest(context);

            var originalBodyStream = context.Response.Body;
            var responseStream = _memoryStreamManager.GetStream();
            context.Response.Body = responseStream;

            Exception? exception = null;
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                exception = ex;

                var response = HandleException(context, ex);

                await context.Response.WriteAsync(response);
            }

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            string text = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            watch.Stop();
            int responseTime = TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds).Seconds;

            string responseMessage = string.Join(Environment.NewLine, "Http Response Information",
                $"Schema: {context.Request.Scheme} ", $"Host: {context.Request.Host} ", $"Path: {context.Request.Path} ",
                $"QueryString: {context.Request.QueryString} ", $"Response Body: {text}");

            var message = string.Join(Environment.NewLine, $"RequestId: {context.TraceIdentifier}",
                $"Response time for completed request: {responseTime} sn", requestMessage,
                exception == null ? null : "Exception:" + exception.Message, responseMessage);

            await responseStream.CopyToAsync(originalBodyStream);

            if (exception == null)
            {
                _logger.LogInformation(message);
            }
            else
            {
                _logger.LogError(message);
            }

        }

        private async Task<string> logRequest(HttpContext context)
        {
            context.Request.EnableBuffering();
            var requestStream = _memoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);

            var message = string.Join(Environment.NewLine, "#### Http Request Information:",
                $"Schema: {context.Request.Scheme}", $"Host: {context.Request.Host}", $"Method: {context.Request.Method}",
                $"Path: {context.Request.Path}", $"QueryString: {context.Request.QueryString}",
                $"Request Body: {readStreamInChunks(requestStream)}");

            context.Request.Body.Position = 0;
            return message;
        }

        private static string readStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;

            stream.Seek(0, SeekOrigin.Begin);

            var textWriter = new StringWriter();
            var reader = new StreamReader(stream);

            var readChunk = new char[readChunkBufferLength];
            int readChunkLength;

            do
            {
                readChunkLength = reader.ReadBlock(readChunk, 0, readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);

            return textWriter.ToString();
        }

        private string HandleException(HttpContext context, Exception exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            ErrorMessage errorMessage = new();
            if (exception is CustomException customException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorMessage = customException.ErrorMessage;
            }
            else
            {
                errorMessage.ErrorDescription = exception.Message;
            }

            var response = new CustomResponseDto().Error(context.Response.StatusCode, errorMessage);

            return JsonSerializer.Serialize(response);
        }
    }
}
