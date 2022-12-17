using AzureFunctions.Extensions.Swashbuckle;
using AzureFunctions.Extensions.Swashbuckle.Settings;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CookbookApi
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {

            builder.AddSwashBuckle(Assembly.GetExecutingAssembly(), opts => 
            {
                opts.AddCodeParameter = true;
                opts.Documents = new[]
                {
                    new SwaggerDocument{
                        Name = "v1",
                        Title = "Swagger",
                        Description = "Cookbook Api",
                        Version = "v1"
                    }
                };
                opts.ConfigureSwaggerGen = x => 
                {
                    x.CustomOperationIds(apiDesc => {
                        return apiDesc.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : default(Guid).ToString();
                    });
                };
            });
        }
    }
}
