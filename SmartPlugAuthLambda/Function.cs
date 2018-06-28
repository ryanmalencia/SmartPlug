using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

using Amazon.Lambda.Core;
using Newtonsoft.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace SmartPlugAuthLambda
{
    public class Function
    {
        public object FunctionHandler(HttpRequest request, ILambdaContext context)
        {
            // Log request to amazon file
            LambdaLogger.Log(request.HttpMethod + Environment.NewLine);

            // If GET request. This happens before the POST request
            // Create the redirect url which contains the client secret created for the skill (the string after "&code=")
            // Log redirect url to amazon file
            // Add response header location to the header with the url to redirect to
            // Create redirect response to send back
            // Send response
            if (request.HttpMethod == "GET")
            {
                var headers = new Dictionary<string, string>();
                var redirectUrl = request.QueryStringParameters["redirect_uri"] + "?state=" + request.QueryStringParameters["state"] + "&code=" + "ryanmalenciatesting";
                LambdaLogger.Log("Redirect url:" + redirectUrl + Environment.NewLine);
                headers.Add(HttpResponseHeader.Location.ToString(), redirectUrl);
                var response = new LambdaResponse()
                {
                    Body = "",
                    StatusCode = HttpStatusCode.Redirect,
                    Headers = headers
                };
                return response;
            }
            // If POST request. This happens after a successful redirect from the GET request above
            // Create ActionTokenRepsonse with TokenType = "bearer"
            // Create OK response to send back with serialized access token
            // Send response
            else if (request.HttpMethod == "POST")
            {
                var AccessTokenResp = new AccessTokenResponse()
                {
                    AccessToken = "AmazonTesting",
                    TokenType = "bearer",
                };
                var response = new LambdaResponse
                {
                    StatusCode = HttpStatusCode.OK, Body = JsonConvert.SerializeObject(AccessTokenResp)
                };
                return response;
            }
            return request;
        }
    }

    public class LambdaResponse
    {
        [JsonProperty("statusCode")]
        public HttpStatusCode StatusCode { get; set; }
        [JsonProperty("headers")]
        public Dictionary<string, string> Headers { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
    }

    public class HttpRequest
    {

        [JsonProperty("resource")]
        public string Resource { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("httpMethod")]
        public string HttpMethod { get; set; }

        [JsonProperty("headers")]
        public Dictionary<string, string> Headers { get; set; }

        [JsonProperty("queryStringParameters")]
        public Dictionary<string, string> QueryStringParameters { get; set; }

        [JsonProperty("pathParameters")]
        public object PathParameters { get; set; }

        [JsonProperty("stageVariables")]
        public object StageVariables { get; set; }

        [JsonProperty("requestContext")]
        public RequestContext RequestContext { get; set; }

        [JsonProperty("body")]
        public object Body { get; set; }

        [JsonProperty("isBase64Encoded")]
        public bool IsBase64Encoded { get; set; }
    }

    public class RequestContext
    {

        [JsonProperty("resourceId")]
        public string ResourceId { get; set; }

        [JsonProperty("resourcePath")]
        public string ResourcePath { get; set; }

        [JsonProperty("httpMethod")]
        public string HttpMethod { get; set; }

        [JsonProperty("extendedRequestId")]
        public string ExtendedRequestId { get; set; }

        [JsonProperty("requestTime")]
        public string RequestTime { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("accountId")]
        public string AccountId { get; set; }

        [JsonProperty("protocol")]
        public string Protocol { get; set; }

        [JsonProperty("stage")]
        public string Stage { get; set; }

        [JsonProperty("requestTimeEpoch")]
        public long RequestTimeEpoch { get; set; }

        [JsonProperty("requestId")]
        public string RequestId { get; set; }

        [JsonProperty("identity")]
        public Identity Identity { get; set; }

        [JsonProperty("apiId")]
        public string ApiId { get; set; }
    }

    public class Identity
    {

        [JsonProperty("cognitoIdentityPoolId")]
        public object CognitoIdentityPoolId { get; set; }

        [JsonProperty("accountId")]
        public object AccountId { get; set; }

        [JsonProperty("cognitoIdentityId")]
        public object CognitoIdentityId { get; set; }

        [JsonProperty("caller")]
        public object Caller { get; set; }

        [JsonProperty("sourceIp")]
        public string SourceIp { get; set; }

        [JsonProperty("accessKey")]
        public object AccessKey { get; set; }

        [JsonProperty("cognitoAuthenticationType")]
        public object CognitoAuthenticationType { get; set; }

        [JsonProperty("cognitoAuthenticationProvider")]
        public object CognitoAuthenticationProvider { get; set; }

        [JsonProperty("userArn")]
        public object UserArn { get; set; }

        [JsonProperty("userAgent")]
        public string UserAgent { get; set; }

        [JsonProperty("user")]
        public object User { get; set; }
    }

    public class AccessTokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
