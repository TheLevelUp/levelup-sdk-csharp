using System;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LevelUpExampleApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region UI Event Handlers

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            const string supportPhoneNumber = "1.855.538.3542";
            const string supportWebSite = "http://support.thelevelup.com";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(". : LevelUp Example App");
            sb.AppendLine(string.Format("Version {0}", LevelUpExampleAppGlobals.AppVersion));
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
            try
            {
                LevelUpExampleAppGlobals.LoadConfigFile();
            }
            catch (Exception ex)
            {
                Helpers.ShowMessageBox(this,
                                       "Config File Load error: " + ex.Message,
                                       MessageBoxButton.OK,
                                       MessageBoxImage.Error);

                SetStatusLabelText("Error on File Load", LevelUpExampleAppGlobals.ERROR_COLOR);
            }

            FileLocationTextBox.Text = LevelUpExampleAppGlobals.LevelUpConfigFilePath;

            if (string.IsNullOrEmpty(LevelUpExampleAppGlobals.ApiKey))
            {
                SetStatusLabelText("Please set LevelUp API Key", LevelUpExampleAppGlobals.ERROR_COLOR);
            }
            else if(!LevelUpData.Instance.IsValid())
            {
                SetStatusLabelText(LevelUpExampleAppGlobals.PLEASE_AUTHENTICATE, LevelUpExampleAppGlobals.ERROR_COLOR);
            }
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

            LevelUpExampleAppGlobals.LevelUpConfigFilePath = FileLocationTextBox.Text;
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            DoLoad();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(LevelUpExampleAppGlobals.LevelUpConfigFilePath))
            {
                SetStatusLabelText("Specify save location", LevelUpExampleAppGlobals.ERROR_COLOR);
                Helpers.ShowMessageBox(this,
                                       "Please specify a save location",
                                       MessageBoxButton.OK,
                                       MessageBoxImage.Error);
                return;
            }

            try
            {
                LevelUpExampleAppGlobals.LevelUpConfigFilePath = Path.GetFullPath(LevelUpExampleAppGlobals.LevelUpConfigFilePath);
            }
            catch (Exception)
            {
                SetStatusLabelText("Invalid save location", LevelUpExampleAppGlobals.ERROR_COLOR);
                return;
            }

            string errorMessages;
            if (LevelUpData.Instance.IsValid(out errorMessages))
            {
                SetStatusLabelText("Validation Failed", LevelUpExampleAppGlobals.ERROR_COLOR);
                Helpers.ShowMessageBox(this, errorMessages, MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            if (!UserHasPermissionToSaveFile(LevelUpExampleAppGlobals.LevelUpConfigFilePath))
            {
                string msg = String.Format("Current user does not have permissions to save configuration to " +
                                           "{0}.{1}{1}Please restart the application and " +
                                           "run it as an administrator.",
                                           LevelUpExampleAppGlobals.LevelUpConfigFilePath,
                                           Environment.NewLine);

                Helpers.ShowMessageBox(this, msg, MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            try
            {
                if (XmlFormatRadioButton.IsChecked.HasValue && XmlFormatRadioButton.IsChecked.Value)
                {
                    LevelUpData.Instance.SerializeToXml(LevelUpExampleAppGlobals.LevelUpConfigFilePath);
                }
                else
                {
                    LevelUpData.Instance.SerializeToJson(LevelUpExampleAppGlobals.LevelUpConfigFilePath);
                }

                SetStatusLabelText("Config Data Saved Successfully!", LevelUpExampleAppGlobals.SUCCESS_COLOR);
            }
            catch (UnauthorizedAccessException)
            {
                // just in case the permissions changed between the time we did the check and
                // the time the save occurred (highly unlikely)
                const string msg = "Insufficient Permissions";
                SetStatusLabelText(msg, LevelUpExampleAppGlobals.ERROR_COLOR);
                Helpers.ShowMessageBox(this,
                                       string.Format("{0}: User does not have permission to save to {1}",
                                                     msg,
                                                     LevelUpExampleAppGlobals.LevelUpConfigFilePath),
                                       MessageBoxButton.OK,
                                       MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                const string msg = "Error on Save";
                SetStatusLabelText(msg, LevelUpExampleAppGlobals.ERROR_COLOR);
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
            LevelUpExampleAppGlobals.LevelUpConfigFilePath = FileLocationTextBox.Text.Trim('"', ' ');
            FileLocationTextBox.Text = LevelUpExampleAppGlobals.LevelUpConfigFilePath;

            if (!File.Exists(LevelUpExampleAppGlobals.LevelUpConfigFilePath))
            {
                SetStatusLabelText("Config file not found", LevelUpExampleAppGlobals.ERROR_COLOR);
                return;
            }

            try
            {
                LevelUpExampleAppGlobals.LoadConfigFile();
            }
            catch (Exception ex)
            {
                Helpers.ShowMessageBox(this,
                                       string.Format("Error loading config file: {0}{1}{2}",
                                                     LevelUpExampleAppGlobals.LevelUpConfigFilePath,
                                                     Environment.NewLine,
                                                     ex.Message),
                                       MessageBoxButton.OK,
                                       MessageBoxImage.Error);

                SetStatusLabelText("Error loading config file", LevelUpExampleAppGlobals.ERROR_COLOR);
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
                StatusLabel.Foreground = color ?? LevelUpExampleAppGlobals.ERROR_COLOR;
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
