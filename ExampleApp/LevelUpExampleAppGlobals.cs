using System.Windows.Media;
using LevelUp.Api.Client;
using System;
using System.IO;
using System.Reflection;
using System.Windows;

namespace LevelUpExampleApp
{
    /// <summary>
    /// Wrapper class that controls access to the config manager so we share it across application
    /// </summary>
    public static class LevelUpExampleAppGlobals
    {
        //TODO: Set your API key here or save it in a text file named api_key.txt in the application folder
        private const string API_KEY = null;

        public const string DEFAULT_CONFIG_FILE_NAME = "LevelUp.Config";

        private static ILevelUpClient _api;
        private static string _levelUpConfigFilePath;
        private static readonly string _currentDirectoryName =
            Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
        private static readonly string _apiKeyFilePath = Path.Combine(_currentDirectoryName, "api_key.txt");


        // default the config to the LevelUp.config in current folder
        private static readonly string _defaultConfigPath;
        private static readonly AssemblyName _assemblyName = typeof (LevelUpExampleAppGlobals).Assembly.GetName();

        public static readonly Brush SUCCESS_COLOR = new SolidColorBrush(Colors.LimeGreen);
        public static readonly Brush ERROR_COLOR = new SolidColorBrush(Colors.IndianRed);

        public static event EventHandler<EventArgs> ConfigLoaded;
        public static event EventHandler<EventArgs> ConfigPathChanged;

        public static ILevelUpClient Api
        {
            get
            {
                return _api ?? (_api = LevelUpClientFactory.Create("LevelUp",
                                                                   "ExampleApp",
                                                                   AppVersion.ToString(),
                                                                   ".NET 3.0"));
            }
        }

        internal static string ApiKey
        {
            get
            {
                return !File.Exists(_apiKeyFilePath) ? API_KEY : File.ReadAllText(_apiKeyFilePath).Trim();
            }
        }

        public static Version AppVersion
        {
            get { return _assemblyName.Version; }
        }

        // Current path to the config file
        public static string LevelUpConfigFilePath
        {
            get { return _levelUpConfigFilePath; }

            set
            {
                if (!string.IsNullOrEmpty(value) &&
                    !_levelUpConfigFilePath.Equals(value, StringComparison.InvariantCultureIgnoreCase))
                {
                    _levelUpConfigFilePath = value;

                    EventHandler<EventArgs> handler = ConfigPathChanged;
                    if (null != handler)
                    {
                        handler(new object(), EventArgs.Empty);
                    }
                }
            }
        }

        static LevelUpExampleAppGlobals()
        {
            _levelUpConfigFilePath = string.Empty;

            string executingDirectory =
                Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath) ?? string.Empty;

            _defaultConfigPath = Path.Combine(executingDirectory, DEFAULT_CONFIG_FILE_NAME);

            LevelUpConfigFilePath = _defaultConfigPath;
        }

        /// <summary>
        /// Loads the configuration from disk
        /// </summary>
        public static void LoadConfigFile()
        {
            if (!File.Exists(LevelUpConfigFilePath))
            {
                return;
            }

            try
            {
                LevelUpData.LoadConfigData(LevelUpConfigFilePath);

                EventHandler<EventArgs> handler = ConfigLoaded;
                if (null != handler)
                {
                    handler(new object(), EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                Helpers.ShowMessageBox(Application.Current.MainWindow,
                                       string.Format("Error while loading configuration from {0}:{1}{2}",
                                                     LevelUpConfigFilePath,
                                                     Environment.NewLine,
                                                     ex.Message),
                                       MessageBoxButton.OK,
                                       MessageBoxImage.Error);
            }
        }
    }
}
