using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Scheduler {
    public class Program {
        public static void Main(string[] args) {
            Console.WriteLine("Iniciando proceso {0}", Process.GetCurrentProcess().Id);
            Environment.GetEnvironmentVariables().OfType<DictionaryEntry>().ToList()
                .ForEach(kvp => Console.WriteLine($"env {kvp.Key}={kvp.Value}"));
            Console.WriteLine("=======================INICIANDO=======================");

            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseUrls("http://*:8080")
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseSetting("detailedErrors", "true")
                .CaptureStartupErrors(true)
                .Build();
    }
}