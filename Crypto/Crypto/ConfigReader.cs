using Crypto.BinanceControllers;
using Crypto.Model;
using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Json;
using Serilog.Sinks.RollingFile;
using System.Configuration;

namespace Crypto
{
    internal class ConfigReader
    {
        internal ILogger GetLoggerFromConfig()
        {
            string path = ConfigurationManager.AppSettings.Get("LogFileDirectory") ?? throw new ArgumentNullException("ConfigValue LogFileDirectory");
            string fileName = ConfigurationManager.AppSettings.Get("LogFileName") ?? throw new ArgumentNullException("ConfigValue LogFileName");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return (new LoggerConfiguration()
                            .Enrich.WithExceptionDetails()
                            .WriteTo.Sink(new RollingFileSink(
                                                path + fileName,
                                                new JsonFormatter(renderMessage: true),
                                                null,
                                                null
                                                )
                                         )
                    ).CreateLogger();
        }

        internal DBController GetDBControllerFromConfig()
        {
            return new DBController(ConfigurationManager.AppSettings.Get("ConnectionString")??throw new ArgumentNullException("ConfigValue ConnectionString"));
        }

        internal IBinanceController GetBinanceControllerFromConfig()
        {
            switch (ConfigurationManager.AppSettings.Get("Mode"))
            {
                case "Dummy":return new DummyBinanceController();
                case "Real": return new BinanceController(new ModelConverter(), GetKeyFromConfig(), GetSecretFromConfig());
                case "GitHub": return new GitHubBinanceController(new ModelConverter(), GetKeyFromConfig(), GetSecretFromConfig());
                default: throw new ArgumentOutOfRangeException("ConfigValue Mode");
            }

        }
        internal string GetKeyFromConfig() => ConfigurationManager.AppSettings.Get("ApiKey");
        internal string GetSecretFromConfig() => ConfigurationManager.AppSettings.Get("ApiSecret");
    }
}
