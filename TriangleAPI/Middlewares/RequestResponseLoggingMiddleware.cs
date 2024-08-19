using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.IO;
using TriangleAPI.Interfaces;
using TriangleAPI.Models.Request;

namespace TriangleAPI.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        private readonly IElasticSearchService _elasticService;
        private readonly IConfiguration _configuration;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, IElasticSearchService elasticService, IConfiguration configuration)
        {
            _next = next;
            _elasticService = elasticService;
            _logger = loggerFactory.CreateLogger<RequestResponseLoggingMiddleware>();
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
            _configuration = configuration;
        }

        public string strElasticIndexName => _configuration["ReqResElasticSearch:IndexName"] + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();

        public async Task Invoke(HttpContext context)
        {
            await LogRequest(context);
            await LogResponse(context);
        }

        private async Task LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();

            using (var requestStream = _recyclableMemoryStreamManager.GetStream())
            {
                await context.Request.Body.CopyToAsync(requestStream).ConfigureAwait(false);
                string strRequest = ReadStreamInChunks(requestStream).Trim();
                string APIName = context.Request.Path.Value.Split('/')[1];

                if (APIName == "scmd" || APIName == "WeatherForecast")
                {
                    _logger.LogInformation($"Http Request Information:{Environment.NewLine}" +
                                       $"Schema:{context.Request.Scheme} " +
                                       $"Host: {context.Request.Host} " +
                                       $"Path: {context.Request.Path} " +
                                       $"QueryString: {context.Request.QueryString} " +
                                       $"Request Body: {ReadStreamInChunks(requestStream)}");

                    context.Request.Body.Position = 0;

                    try
                    {
                        var requestBody = JsonConvert.DeserializeObject<JObject>(strRequest);
                        var ReqResponseGenerate = new ApiLogRequest()
                        {
                            RequestResponseData = JsonConvert.SerializeObject(requestBody),
                            Host = context.Request.Host.ToString(),
                            IsRequest = true,
                            TraceIdentifier = context.TraceIdentifier,
                            Method = context.Request.Method,
                            Path = context.Request.Path,
                            PathBase = context.Request.PathBase,
                            Protocol = context.Request.Protocol,
                            QueryString = context.Request.QueryString.ToString(),
                            Scheme = context.Request.Scheme,
                            TimeStamp = new DateTimeOffset(DateTime.Now),
                        };

                        //var IndexRequest = new IndexRequest<object>(index: strElasticIndexName)
                        //{
                        //    Document = ReqResponseGenerate
                        //};

                        await _elasticService.InsertDocumentAsync<ApiLogRequest>(ReqResponseGenerate, strElasticIndexName).ConfigureAwait(false);
                    }
                    catch (Exception e)
                    {
                        //log error
                    }
                }
            }
        }

        private async Task LogResponse(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;

            using (var responseBody = _recyclableMemoryStreamManager.GetStream())
            {
                context.Response.Body = responseBody;

                await _next(context).ConfigureAwait(false);

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var text = await new StreamReader(context.Response.Body).ReadToEndAsync().ConfigureAwait(false);
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                string APIName = context.Request.Path.Value.Split('/')[1];
                _logger.LogInformation($"Http Response Information:{Environment.NewLine}" +
                                       $"Schema:{context.Request.Scheme} " +
                                       $"Host: {context.Request.Host} " +
                                       $"Path: {context.Request.Path} " +
                                       $"QueryString: {context.Request.QueryString} " +
                                       $"Response Body: {text}");

                await responseBody.CopyToAsync(originalBodyStream).ConfigureAwait(false);

                try
                {
                    if (APIName == "scmd" || APIName == "WeatherForecast")
                    {
                        var requestBody = JsonConvert.DeserializeObject<object>(text);
                        var ReqResponseGenerate = new ApiLogRequest()
                        {
                            RequestResponseData = JsonConvert.SerializeObject(requestBody),
                            Host = context.Request.Host.ToString(),
                            IsRequest = false,
                            TraceIdentifier = context.TraceIdentifier,
                            Method = context.Request.Method,
                            Path = context.Request.Path,
                            PathBase = context.Request.PathBase,
                            Protocol = context.Request.Protocol,
                            QueryString = context.Request.QueryString.ToString(),
                            Scheme = context.Request.Scheme,
                            TimeStamp = new DateTimeOffset(DateTime.Now),

                        };

                        //var IndexRequest = new IndexRequest<object>(index: strElasticIndexName)
                        //{
                        //    Document = ReqResponseGenerate
                        //};

                        await _elasticService.InsertDocumentAsync<ApiLogRequest>(ReqResponseGenerate, strElasticIndexName).ConfigureAwait(false);
                    }
                }
                catch (Exception e)
                {
                    //log error
                }
            }
        }

        private static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;

            stream.Seek(0, SeekOrigin.Begin);

            using var textWriter = new StringWriter();
            using var reader = new StreamReader(stream, leaveOpen: true);

            var readChunk = new char[readChunkBufferLength];
            int readChunkLength;

            do
            {
                readChunkLength = reader.ReadBlock(readChunk, 0, readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);

            return textWriter.ToString();
        }
    }

}
