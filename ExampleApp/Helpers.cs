using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;

namespace LevelUpExampleApp
{
    internal static class Helpers
    {
        internal static void ShowMessageBox(Window window,
                                            string message,
                                            MessageBoxButton buttons = MessageBoxButton.OK,
                                            MessageBoxImage icon = MessageBoxImage.Information)
        {
            MessageBox.Show(window, message, "LevelUp Example App", buttons, icon);
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
