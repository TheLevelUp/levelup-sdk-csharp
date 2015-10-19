using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;

namespace LevelUpConfigTool
{
    internal static class Helpers
    {
        private const string API_KEY = "Your LevelUp API Key goes here."; //TODO: Set your API key here or save it in a text file named api_key.txt in the application folder

        internal static string GetApiKey()
        {
            string currentDirectoryName = 
                Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);

            string apiKeyFilePath = Path.Combine(currentDirectoryName, "api_key.txt");

            string apiKey = null;

            if (File.Exists(apiKeyFilePath))
            {
                apiKey = File.ReadAllText(apiKeyFilePath).Trim();
            }

            return apiKey;
        }

        internal static void ShowMessageBox(Window window,
                                            string message,
                                            MessageBoxButton buttons = MessageBoxButton.OK,
                                            MessageBoxImage icon = MessageBoxImage.Information)
        {
            MessageBox.Show(window, message, "LevelUp Configuration Tool", buttons, icon);
        }

        internal static bool VerifyIsInt(ref StringBuilder sb, string valueToVerify, string name)
        {
            if (!VerifyIsNotNullOrEmpty(ref sb, valueToVerify, name))
            {
                return false;
            }

            bool isInt = false;
            int intVal;

            if (!int.TryParse(valueToVerify, out intVal))
            {
                sb.AppendLine(string.Format("{0} must be an integer! \"{1}\" is not a valid integer value.",
                                            name,
                                            valueToVerify));
            }
            else
            {
                isInt = true;
            }

            return isInt;
        }

        internal static bool VerifyIsNotNullOrEmpty(ref StringBuilder sb, string valueToVerify, string name)
        {
            bool isNotNullOrEmpty = false;

            if (string.IsNullOrEmpty(valueToVerify))
            {
                sb.AppendLine(string.Format("{0} must be set! Cannot be null or empty.", name));
            }
            else
            {
                isNotNullOrEmpty = true;
            }

            return isNotNullOrEmpty;
        }
    }
}
