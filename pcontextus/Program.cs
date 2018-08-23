using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.ML;
using pcontextus.Training;
using Serilog.Extensions.Logging;

namespace pcontextus
{
    public class Program
    {
       public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
            
        }



      

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)

             .ConfigureAppConfiguration((hostingContext, config) =>
             {
                 var env = hostingContext.HostingEnvironment;

                 config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);


                 config.AddEnvironmentVariables();
             })
            .ConfigureLogging((hostingContext, logging) =>
            {
                logging.AddProvider(new SerilogLoggerProvider(dispose: true));
            })
                
            .UseStartup<Startup>()
                .Build();
    }
}
