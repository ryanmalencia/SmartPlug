using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace SmartPLugHandlerLambda
{
    public class Function
    {
        public object FunctionHandler(object input, ILambdaContext context)
        {
            // Log request to amazon file
            LambdaLogger.Log(input.ToString() + Environment.NewLine);
            return JsonConvert.SerializeObject(new Response());
        }

        public class Header
        {
            [JsonProperty("namespace")]
            public string Namespace{ get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("payloadVersion")]
            public string PayloadVersion { get; set; }

            [JsonProperty("messageId")]
            public string MessageId { get; set; }
        }

        public class Cookie
        {
            [JsonProperty("detail1")]
            public string Detail1 { get; set; }

            [JsonProperty("detail2")]
            public string Detail2 { get; set; }
        }

        public class Supported
        {
            [JsonProperty("name")]
            public string Name { get; set; }
        }

        public class Properties
        {
            [JsonProperty("supported")]
            public IList<Supported> Supported { get; set; }

            [JsonProperty("proactivelyReported")]
            public bool ProactivelyReported { get; set; }

            [JsonProperty("retrievable")]
            public bool Retrievable { get; set; }
        }

        public class Resolution
        {
            [JsonProperty("width")]
            public int Width { get; set; }

            [JsonProperty("height")]
            public int Height { get; set; }
        }

        public class CameraStreamConfiguration
        {
            [JsonProperty("protocols")]
            public IList<string> Protocols { get; set; }

            [JsonProperty("resolutions")]
            public IList<Resolution> Resolutions { get; set; }

            [JsonProperty("authorizationTypes")]
            public IList<string> AuthorizationTypes { get; set; }

            [JsonProperty("videoCodecs")]
            public IList<string> VideoCodecs { get; set; }

            [JsonProperty("audioCodecs")]
            public IList<string> AudioCodecs { get; set; }
        }

        public class Capability
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("interface")]
            public string Interface{get; set; }

            [JsonProperty("version")]
            public string Version { get; set; }

            [JsonProperty("properties")]
            public Properties Properties { get; set; }

            [JsonProperty("supportsDeactivation")]
            public bool? SupportsDeactivation { get; set; }

            [JsonProperty("proactivelyReported")]
            public bool? ProactivelyReported { get; set; }

            [JsonProperty("cameraStreamConfigurations")]
            public IList<CameraStreamConfiguration> CameraStreamConfigurations { get; set; }
        }

        public class Endpoint
        {
            [JsonProperty("endpointId")]
            public string EndpointId { get; set; }

            [JsonProperty("manufacturerName")]
            public string ManufacturerName { get; set; }

            [JsonProperty("friendlyName")]
            public string FriendlyName { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("displayCategories")]
            public IList<string> DisplayCategories { get; set; }

            [JsonProperty("cookie")]
            public Cookie Cookie { get; set; }

            [JsonProperty("capabilities")]
            public IList<Capability> Capabilities { get; set; }
        }

        public class Payload
        {
            [JsonProperty("endpoints")]
            public IList<Endpoint> Endpoints { get; set; }
        }

        public class Response
        {

            [JsonProperty("header")]
            public Header Header { get; set; }

            [JsonProperty("payload")]
            public Payload Payload { get; set; }
        }
    }
}