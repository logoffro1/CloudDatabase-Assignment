using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ExtraFunction.Authorization
{
    public class JWTMiddleware : IFunctionsWorkerMiddleware
    {
        ILogger Logger { get; }

        public JWTMiddleware(ILogger<JWTMiddleware> Logger)
        {
            this.Logger = Logger;
        }

        public async Task Invoke(FunctionContext Context, FunctionExecutionDelegate Next)
        {
            string headerString;
            Dictionary<string, string> Headers = new Dictionary<string, string>();
            if (Context.BindingContext.BindingData.TryGetValue("Headers", out object? headerValues))
            {
                headerString = headerValues.ToString();
                Headers = JsonConvert.DeserializeObject<Dictionary<string, string>>(headerString);
            }

            await Next(Context);
        }

    }
}
