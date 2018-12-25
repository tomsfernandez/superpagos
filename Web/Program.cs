using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Web {
    public class Program {
        
        public static void Main(string[] args) {
            Console.WriteLine("Iniciando proceso {0}", Process.GetCurrentProcess().Id);
            Environment.GetEnvironmentVariables().OfType<DictionaryEntry>().ToList()
                .ForEach(kvp => Console.WriteLine($"env {kvp.Key}={kvp.Value}"));
            Console.WriteLine("=======================INICIANDO=======================");

            BuildWebHost(args).Run();
        }

        private static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseUrls("http://*:5000")
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseSetting("detailedErrors", "true")
                .CaptureStartupErrors(true)
                .Build();
    }
}