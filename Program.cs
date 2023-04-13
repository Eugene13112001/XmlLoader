using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Globalization;
using XmlLoader.Database;
using XmlLoader.Services;
using XmlLoader.Services.Impl;

namespace XmlLoader
{
    internal class Program
    {
        private static IHost? _host;

        public static IHost Hosting => _host ??= CreateHostBuilder(Environment.GetCommandLineArgs()).Build();
        public static IServiceProvider Services => Hosting.Services;

        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
            .ConfigureHostConfiguration(options =>
                options.AddJsonFile("appsettings.json", true))
            .ConfigureAppConfiguration(options =>
                options.AddJsonFile("appsettings.json", true)
                .AddEnvironmentVariables()
                .AddCommandLine(args))
            .ConfigureLogging(options =>
                options.ClearProviders()
                    .AddConsole()
                    .AddDebug())
            .ConfigureServices(ConfigureServices);
        protected static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(host.Configuration.GetConnectionString("DatabaseConnection"))
            )
            .AddTransient<IDocumentLoaderService, DocumentLoader>()
            .AddSingleton<IEntrypointInfo, SystemEnvironmentEntrypointInfo>();
        }

        public static async Task UploadDocumentAsync()
        {
            await using var servicesScope = Services.CreateAsyncScope();
            var serviceProvider = servicesScope.ServiceProvider;
            var systemEnvironments = serviceProvider.GetService<IEntrypointInfo>();
            if (systemEnvironments?.CommandLineArgs.Count > 1 &&
                File.Exists(systemEnvironments.CommandLineArgs[1]))
            {
                var documentLoaderService = serviceProvider.GetService<IDocumentLoaderService>();
                await documentLoaderService!.UploadAsync(systemEnvironments.CommandLineArgs[1]);
            }   
        }

        static async Task Main(string[] args)
        {
            #region Configure Global Application Settings

            var cultureName = Thread.CurrentThread.CurrentCulture.Name;
            var cultureInfo = new CultureInfo(cultureName);
            if (cultureInfo.NumberFormat.NumberDecimalSeparator != ".")
            {
                cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
                Thread.CurrentThread.CurrentCulture = cultureInfo;
            }

            #endregion

            await Hosting.StartAsync();
            await UploadDocumentAsync();
            await Hosting.StopAsync();
        }
    }

    public class SystemEnvironmentEntrypointInfo : IEntrypointInfo
    {
        public string CommandLine => Environment.CommandLine;

        public IReadOnlyList<string> CommandLineArgs => Environment.GetCommandLineArgs();
    }

    public interface IEntrypointInfo
    {
        string CommandLine { get; }

        IReadOnlyList<string> CommandLineArgs { get; }
        bool HasFlag(string flagName)
        {
            return CommandLineArgs.Any(a => ("-" + a) == flagName || ("/" + a) == flagName);
        }
    }
}