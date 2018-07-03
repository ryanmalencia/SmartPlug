using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SmartPLugHandlerLambda.Request
{
    public class PowerRequest
    {
        [JsonProperty("directive")]
        public Directive Directive { get; set; }
    }

    public class Header
    {

        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("payloadVersion")]
        public string PayloadVersion { get; set; }

        [JsonProperty("messageId")]
        public string MessageId { get; set; }

        [JsonProperty("correlationToken")]
        public string CorrelationToken { get; set; }
    }

    public class Scope
    {

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }

    public class Cookie
    {
    }

    public class Endpoint
    {

        [JsonProperty("scope")]
        public Scope Scope { get; set; }

        [JsonProperty("endpointId")]
        public string EndpointId { get; set; }

        [JsonProperty("cookie")]
        public Cookie Cookie { get; set; }
    }

    public class Payload
    {
    }

    public class Directive
    {

        [JsonProperty("header")]
        public Header Header { get; set; }

        [JsonProperty("endpoint")]
        public Endpoint Endpoint { get; set; }

        [JsonProperty("payload")]
        public Payload Payload { get; set; }
    }
}
