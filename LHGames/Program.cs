using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace LHGames
{
    public class Program
    {
        public static void Main(string[] args)
        {

            try
            {
                DotNetEnv.Env.Load();
            }
            catch (Exception)
            {
                Console.WriteLine("No .env file was found, mode offline activated");
            }

            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) => WebHost.CreateDefaultBuilder(args)
                        .UseStartup<Startup>()
                        .UseIISIntegration()
                        .UseUrls($"http://*:{Environment.GetEnvironmentVariable("PORT") ?? "3000"}")
                        .UseKestrel()
                        .Build();
    }
}
