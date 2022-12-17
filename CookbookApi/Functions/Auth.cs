using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using CookbookApi.Models;
using CookbookApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace CookbookApi.Functions
{
    public class Auth
    {
        private readonly ILogger<Auth> _logger;

        public Auth(ILogger<Auth> log)
        {
            _logger = log;
        }

        [FunctionName("Auth")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "userCredentials" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UserCredentials), Required = true, Description = "UserCredentials object that needs to be authenticated")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "auth")] HttpRequest req, UserCredentials userCredentials, ILogger log)
        {
            bool authenticated = userCredentials?.UserName.Equals("dave", StringComparison.InvariantCultureIgnoreCase) ?? false;
            if (!authenticated)
            {
                return await Task.FromResult(new UnauthorizedResult()).ConfigureAwait(false);
            }
            else
            {
                GenerateJWTToken generateJWTToken = new();
                string token = generateJWTToken.IssuingJWT(userCredentials.UserName);
                return await Task.FromResult(new OkObjectResult(token)).ConfigureAwait(false);
            }
        }
    }
}

