using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SmartPLugHandlerLambda.Response
{
    public class PowerResponse
    {
        [JsonProperty("context")]
        public Context Context { get; set; }

        [JsonProperty("event")]
        public Event Event { get; set; }
    }

    public class Property
    {

        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public object Value { get; set; }

        [JsonProperty("timeOfSample")]
        public DateTime TimeOfSample { get; set; }

        [JsonProperty("uncertaintyInMilliseconds")]
        public int UncertaintyInMilliseconds { get; set; }
    }

    public class Context
    {

        [JsonProperty("properties")]
        public IList<Property> Properties { get; set; }
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

    public class Endpoint
    {

        [JsonProperty("scope")]
        public Scope Scope { get; set; }

        [JsonProperty("endpointId")]
        public string EndpointId { get; set; }
    }

    public class Payload
    {
    }

    public class Event
    {

        [JsonProperty("header")]
        public Header Header { get; set; }

        [JsonProperty("endpoint")]
        public Endpoint Endpoint { get; set; }

        [JsonProperty("payload")]
        public Payload Payload { get; set; }
    }
}
