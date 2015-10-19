using System.Windows;
using LevelUp.Integrations.Configuration;
using LevelUp.Api.Client;
using System;
using System.IO;
using System.Reflection;

namespace LevelUpConfigTool
{
    /// <summary>
    /// Wrapper class that controls access to the config manager so we share it across application
    /// </summary>
    public static class LevelUpConfigGlobals
    {
        private static ILevelUpClient _api;
        private static string _levelUpConfigFilePath;

        public static event EventHandler<EventArgs> ConfigLoaded;

        public static event EventHandler<EventArgs> ConfigPathChanged;

        public static event EventHandler<EventArgs> RequestedRevenueCenterPrompt;

        // default the config to the LevelUp.config in current folder
        private static readonly string DefaultConfigPath;

        public static ILevelUpClient Api
        {
            get
            {
                return _api ?? (_api = LevelUpClientFactory.Create("LevelUp",
                                                                   "ConfigurationApp",
                                                                   MainWindow.CONFIG_TOOL_VERSION.ToString(),
                                                                   ".NET 3.0"));
            }
        }
          
        // Current path to the config file
        public static string LevelUpConfigFilePath
        {
            get { return _levelUpConfigFilePath; }

            private set
            {
                if (!string.IsNullOrEmpty(value) &&
                    !_levelUpConfigFilePath.Equals(value, StringComparison.InvariantCultureIgnoreCase))
                {
                    _levelUpConfigFilePath = value;

                    if (ConfigPathChanged != null)
                    {
                        ConfigPathChanged(new object(), EventArgs.Empty);
                    }

                }
            }
        }

        public static LevelUpData LevelUpData { get; private set; }

        static LevelUpConfigGlobals()
        {
            _levelUpConfigFilePath = string.Empty;

            string executingDirectory =
                Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath) ?? string.Empty;

            DefaultConfigPath = Path.Combine(executingDirectory, "LevelUp.Config");

            LoadConfigFile(DefaultConfigPath);
        }

        /// <summary>
        /// Loads the configuration from disk
        /// </summary>
        /// <param name="path">The path to the config file</param>
        public static void LoadConfigFile(string path)
        {
            LevelUpConfigFilePath = path;
            LevelUpData = new LevelUpData();

            if (!File.Exists(path))
            {
                return;
            }

            try
            {
                LevelUpData = LevelUpConfigManager.Get<LevelUpData>(LevelUpConfigFilePath);

                if (null != ConfigLoaded)
                {
                    ConfigLoaded(new object(), EventArgs.Empty);
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

        public static void DisplayRevenueCenterPrompt()
        {
            if (RequestedRevenueCenterPrompt != null)
            {
                RequestedRevenueCenterPrompt(new object(), EventArgs.Empty);
            }
        }
    }
}
