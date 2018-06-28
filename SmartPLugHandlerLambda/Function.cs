using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;

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
            return null;
        }
    }
}
