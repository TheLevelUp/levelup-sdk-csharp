using ConfigurationTool;
using LevelUp.Integrations;
using LevelUp.Integrations.Configuration;
using LevelUp.Api.Client.Models.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LevelUpConfigTool
{
    public partial class MainWindow : Window
    {
        public const string DEFAULT_CONFIG_FILE_NAME = "LevelUp.Config";
        internal static readonly Version CONFIG_TOOL_VERSION = new Version(1, 0, 0, 0);
        private string _pathToConfigFile = Path.Combine(Environment.CurrentDirectory, DEFAULT_CONFIG_FILE_NAME);
        public static readonly Brush SUCCESS_COLOR = new SolidColorBrush(Colors.LimeGreen);
        public static readonly Brush ERROR_COLOR = new SolidColorBrush(Colors.IndianRed);

        public MainWindow()
        {
            InitializeComponent();
        }

        private LevelUpData LevelUpData
        {
            get { return LevelUpConfigGlobals.LevelUpData; }
        }

        #region UI Event Handlers

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            const string supportPhoneNumber = "1.855.538.3542";
            const string supportWebSite = "http://support.thelevelup.com";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(". : LevelUp Configuration Tool");
            sb.AppendLine(string.Format("Version {0}", CONFIG_TOOL_VERSION));
            sb.AppendLine(string.Format("{0}{0}For assistance please call us at {1}",
                                        Environment.NewLine,
                                        supportPhoneNumber));
            sb.AppendLine(string.Format("{0}Or visit our support page at {1}",
                                        Environment.NewLine,
                                        supportWebSite));

            Helpers.ShowMessageBox(this, sb.ToString(), MessageBoxButton.OK, MessageBoxImage.Question);
        }

        private void ConfigTabControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(_pathToConfigFile))
            {
                try
                {
                    LevelUpConfigGlobals.LoadConfigFile(_pathToConfigFile);
                }
                catch (Exception ex)
                {
                    Helpers.ShowMessageBox(this,
                                           "Config File Load error: " + ex.Message,
                                           MessageBoxButton.OK,
                                           MessageBoxImage.Error);

                    SetStatusLabelText("Error on File Load", ERROR_COLOR);
                }
            }

            FileLocationTextBox.Text = _pathToConfigFile;
        }

        private void FileLocationTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                DoLoad();
            }
        }

        private void FileLocationTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(FileLocationTextBox.Text))
            {
                LoadButton.IsEnabled = true;
            }
            _pathToConfigFile = FileLocationTextBox.Text;
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            DoLoad();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_pathToConfigFile))
            {
                SetStatusLabelText("Specify save location", ERROR_COLOR);
                Helpers.ShowMessageBox(this,
                                       "Please specify a save location",
                                       MessageBoxButton.OK,
                                       MessageBoxImage.Error);
                return;
            }

            try
            {
                _pathToConfigFile = Path.GetFullPath(_pathToConfigFile);
            }
            catch (Exception)
            {
                SetStatusLabelText("Invalid save location", ERROR_COLOR);
                return;
            }

            List<string> errorMessages = new List<string>();
            List<IConfigSection> configurationsToSave = new List<IConfigSection>();

            foreach (TabItem tab in ConfigTabControl.Items)
            {
                ISavable savable = tab as ISavable;

                if (null == savable)
                {
                    continue;
                }

                string[] errors;

                if (savable.IsValid(out errors))
                {
                    configurationsToSave.Add(savable.GetData());
                }
                else
                {
                    errorMessages.AddRange(errors);
                }
            }

            string tippingErrorMessage;

            if (!TipsConfiguredCorrectly(configurationsToSave, out tippingErrorMessage))
            {
                errorMessages.Add(tippingErrorMessage);
            }

            if (errorMessages.Count > 0)
            {
                SetStatusLabelText("Validation Failed", ERROR_COLOR);
                string msg = String.Join(Environment.NewLine, errorMessages.ToArray());
                Helpers.ShowMessageBox(this, msg, MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            if (!UserHasPermissionToSaveFile(_pathToConfigFile))
            {
                string msg = String.Format("Current user does not have permissions to save configuration to " +
                                           "{0}.{1}{1}Please restart the application and " +
                                           "run it as an administrator.",
                                           _pathToConfigFile,
                                           Environment.NewLine);

                Helpers.ShowMessageBox(this, msg, MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            Serializer.SerializationFormat format =
                XmlFormatRadioButton.IsChecked.HasValue && XmlFormatRadioButton.IsChecked.Value
                    ? Serializer.SerializationFormat.Xml
                    : Serializer.SerializationFormat.Json;

            try
            {
                LevelUpConfigManager.Save(configurationsToSave,
                                          _pathToConfigFile,
                                          format);

                SetStatusLabelText("Config Data Saved Successfully!", SUCCESS_COLOR);
            }
            catch (UnauthorizedAccessException)
            {
                // just in case the permissions changed between the time we did the check and
                // the time the save occurred (highly unlikely)
                const string msg = "Insufficient Permissions";
                SetStatusLabelText(msg, ERROR_COLOR);
                Helpers.ShowMessageBox(this,
                                       string.Format("{0}: User does not have permission to save to {1}",
                                                     msg,
                                                     _pathToConfigFile),
                                       MessageBoxButton.OK,
                                       MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                const string msg = "Error on Save";
                SetStatusLabelText(msg, ERROR_COLOR);
                Helpers.ShowMessageBox(this,
                                       string.Format("{0}: {1}", msg, ex.Message),
                                       MessageBoxButton.OK,
                                       MessageBoxImage.Error);
            }
        }

        #endregion UI Event Handlers

        #region Helpers

        private void DoLoad()
        {
            _pathToConfigFile = FileLocationTextBox.Text.Trim('"', ' ');
            FileLocationTextBox.Text = _pathToConfigFile;

            if (!File.Exists(_pathToConfigFile))
            {
                SetStatusLabelText("Config file not found", ERROR_COLOR);
                return;
            }

            try
            {
                LevelUpConfigGlobals.LoadConfigFile(_pathToConfigFile);
            }
            catch (Exception ex)
            {
                Helpers.ShowMessageBox(this,
                                       string.Format("Error loading config file: {0}{1}{2}",
                                                     _pathToConfigFile,
                                                     Environment.NewLine,
                                                     ex.Message),
                                       MessageBoxButton.OK,
                                       MessageBoxImage.Error);
                SetStatusLabelText("Error loading config file", ERROR_COLOR);
            }
        }

        internal void ClearStatusLabel()
        {
            StatusLabel.Content = string.Empty;
        }

        internal void SetStatusLabelText(string text, Brush color = null)
        {
            if(!string.IsNullOrEmpty(text))
            {
                StatusLabel.Foreground = color ?? ERROR_COLOR;
                StatusLabel.Content = text;
            }
        }

        private bool UserHasPermissionToSaveFile(string pathToConfigFile)
        {
            FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.Write, pathToConfigFile);

            return SecurityManager.IsGranted(permission);
        }

        #endregion Helpers
    }
}
