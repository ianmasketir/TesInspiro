using Microsoft.Extensions.Configuration;

namespace TesInspiro.Helper
{
    public static class AppConfig
    {
        private static IConfigurationRoot _configuration;
        public static IConfigurationRoot Configuration
        {
            get
            {
                if (_configuration != null)
                    return _configuration;

                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
                _configuration = builder.Build();
                return _configuration;
            }
        }

        public static IConfigurationRoot GetCurrentDeirectory(string directory)
        {
            if (_configuration != null)
                return _configuration;

            var builder = new ConfigurationBuilder().SetBasePath(directory).AddJsonFile("appsettings.json");
            _configuration = builder.Build();
            return _configuration;
        }

        public static AppConfiguration Config
        {
            get
            {
                return new AppConfiguration();
            }
        }

        public class AppConfiguration
        {
            public AppConfiguration()
            {
                string basePath = Directory.GetCurrentDirectory();
                IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                   .SetBasePath(basePath)
                   .AddJsonFile("appsettings.json")
                   .Build();
                GetConfig(configurationRoot);
            }

            #region Get Configuration
            private void GetConfig(IConfiguration configuration)
            {
                Environment = configuration.GetSection("Environment").Value;
                ConfigDev = configuration.GetSection("Dev").Get<Cfg_Dev>();
            }

            public string Environment { get; set; }
            public Cfg_Dev ConfigDev { get; set; }

            public class Cfg_Dev
            {
                public string connection { get; set; }
            }
            #endregion
        }
    }
}