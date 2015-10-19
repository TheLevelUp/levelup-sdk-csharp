using System.Globalization;
using ConfigurationTool;
using LevelUp.Api.Http;
using LevelUp.Integrations.Configuration;
using LevelUp.Integrations.ConfigurationTool;
using LevelUp.Api.Client;
using LevelUp.Api.Client.Models.Responses;
using LevelUpConfigTool.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace LevelUpConfigTool
{
    public partial class LevelUpTabItem : TabItem
    {
        private MainWindow _parentWin;
        private const string PLEASE_AUTHENTICATE = "Please Authenticate";

        private MainWindow ParentWindow
        {
            get { return _parentWin ?? (_parentWin = Window.GetWindow(this) as MainWindow); }
        }

        public LevelUpTabItem()
        {
            InitializeComponent();

            this.DataContext = this;
            this.Locations = new ObservableCollection<LocationViewModel>();

            LevelUpConfigGlobals.ConfigLoaded += LevelUpConfigGlobals_ConfigLoaded;
        }

        private LevelUpData LevelUpData
        {
            get { return LevelUpConfigGlobals.LevelUpData; }
        }

        public ObservableCollection<LocationViewModel> Locations { get; private set; }

        public IConfigSection GetData()
        {
            // because the person may have checked/unchecked the Encrypt Access Token button
            // we need to reset the access token
            LevelUpData.SetAccessToken(LevelUpData.AccessToken, 
                                       ParentWindow.EncryptSensitiveDataCheckbox.IsChecked.GetValueOrDefault(false));

            LevelUpData.PartialAuthEnabled = PartialAuthAllowedCheckBox.IsChecked.GetValueOrDefault(true);

            return LevelUpData;
        }

        #region Delegates

        private delegate AccessToken AuthenticateAsyncDelegate(string username, string password);

        private delegate void UpdateLocationsPropertyDelegate(IList<Location> locations);

        private delegate IList<Location> GetLocationsAsyncDelegate(string levelUpAccessToken, int merchantId);

        private delegate void ShowMessageBoxDelegate(string message, MessageBoxButton button, MessageBoxImage image);

        private delegate void UpdateFieldsDelegate(string levelUpAccessToken, int? merchantId, bool isMerchant);

        #endregion Delegates

        private void LevelUpConfigGlobals_ConfigLoaded(object sender, EventArgs e)
        {
            string[] errorMessages;

            // always default to having the access token checkbox checked
            ParentWindow.EncryptSensitiveDataCheckbox.IsChecked
                = LevelUpData.AccessTokenEncrypted.GetValueOrDefault(true);

            if (!LevelUpData.IsValid(out errorMessages))
            {
                SetStatusLabelText(PLEASE_AUTHENTICATE, MainWindow.ERROR_COLOR);
                return;
            }

            if (!string.IsNullOrEmpty(LevelUpData.AccessToken))
            {
                SetStatusLabelText("Access Token Loaded", MainWindow.SUCCESS_COLOR);
            }

            FillMerchantData(LevelUpData.MerchantId);

            IList<Location> locations = GetLocations(LevelUpData.AccessToken,
                                                     LevelUpData.MerchantId.GetValueOrDefault(0));

            UpdateLocationsProperty(locations);

            LocationDetails details = GetLocationDetails(LevelUpData.AccessToken,
                                                         LevelUpData.LocationId.GetValueOrDefault(0));

            if (null == details)
            {
                FillLocationData(LevelUpData.LocationId);
            }
            else
            {
                FillLocationData(details.LocationId,
                                 locationAddress: details.Address.ToString(),
                                 merchantName: details.MerchantName);
            }

            PartialAuthAllowedCheckBox.IsChecked = LevelUpData.PartialAuthEnabled;

            //Enable controls
            LocationsComboBox.IsEnabled = true;
            ReloadLocationDataButton.IsEnabled = true;
            SetSaveButtonState(true);
        }

        public bool IsValid(out string[] errorMessages)
        {
            StringBuilder messages = new StringBuilder();

            if (string.IsNullOrEmpty(LevelUpData.AccessToken))
            {
                messages.AppendLine("LevelUp Access Token Needed! Please Authenticate.");
            }

            string merchantIdText = MerchantIdValueLabel.Content.ToString().Trim();
            if (Helpers.VerifyIsInt(ref messages, merchantIdText, "LevelUp Merchant ID"))
            {
                LevelUpData.MerchantId = int.Parse(merchantIdText);
            }

            string locationIdText = LocationIdValueLabel.Content.ToString().Trim();
            if (Helpers.VerifyIsInt(ref messages, locationIdText, "LevelUp Location ID"))
            {
                LevelUpData.LocationId = int.Parse(locationIdText);
            }

            errorMessages = messages.Length > 0 ? new[] {messages.ToString()} : new string[0];

            return messages.Length == 0;
        }

        #region UI Event Handlers

        private void AuthenticateButton_Click(object sender, RoutedEventArgs e)
        {
            DoAuthenticateButtonClick();
        }

        private void OnLocationSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LocationsComboBox.SelectedItem == null || !(LocationsComboBox.SelectedItem is LocationViewModel))
            {
                return;
            }

            int locationId = (LocationsComboBox.SelectedItem as LocationViewModel).LocationId;

            LocationDetails details = GetLocationDetails(LevelUpData.AccessToken, locationId);

            //Fill merchant name label & location info
            if (null != details)
            {
                FillLocationData(details.LocationId, details.Address.ToString(), details.MerchantName);
                LevelUpData.LocationId = details.LocationId;
                LevelUpData.MerchantName = details.MerchantName;
            }
            else
            {
                FillLocationData(locationId);
                LevelUpData.LocationId = locationId;
            }

            SetSaveButtonState(true);
        }

        private void PasswordBoxGotFocus(object sender, RoutedEventArgs e)
        {
            PasswordTextBox.Clear();
        }

        private void PasswordBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                DoAuthenticateButtonClick();
            }
        }

        private void ReloadLocationDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (null == LevelUpData || string.IsNullOrEmpty(LevelUpData.AccessToken))
            {
                ShowMessageBox("Please authenticate!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            //Clear the content
            LocationIdValueLabel.Content = string.Empty;
            LocationAddressValueLabel.Content = string.Empty;
            LocationsGroupBox.Visibility = Visibility.Collapsed;

            //Disable the save button
            SetSaveButtonState(false);

            //Get locations async then fill the combo box on callback
            GetLocationsAsyncDelegate getLocationsAsync = new GetLocationsAsyncDelegate(this.GetLocations);

            IAsyncResult result = getLocationsAsync.BeginInvoke(LevelUpData.AccessToken,
                                                                LevelUpData.MerchantId.GetValueOrDefault(0),
                                                                new AsyncCallback(FillLocationsComboBoxCallback),
                                                                getLocationsAsync);
        }

        private void UserNameTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                DoAuthenticateButtonClick();
            }
        }

        #endregion UI Event Handlers

        #region LevelUp Api Calls

        private AccessToken Authenticate(string username, string password)
        {
            AccessToken token = null;

            try
            {
                token = LevelUpConfigGlobals.Api.Authenticate(Helpers.GetApiKey(), username, password);
            }
            catch (LevelUpApiException luEx)
            {
                ShowMessageBoxOnMainUiThread(string.Format("LevelUp Authentication failed!{0}{0}{1}",
                                                           Environment.NewLine,
                                                           luEx.Message),
                                             MessageBoxButton.OK,
                                             MessageBoxImage.Error);
            }

            return token;
        }

        private LocationDetails GetLocationDetails(string levelUpAccessToken, int locationId)
        {
            LocationDetails details = null;

            try
            {
                details = LevelUpConfigGlobals.Api.GetLocationDetails(levelUpAccessToken, locationId);
            }
            catch (LevelUpApiException)
            {
                //Swallow exceptions
            }

            return details;
        }

        private IList<Location> GetLocations(string levelUpAccessToken, int merchantId)
        {
            IList<Location> locations = null;

            try
            {
                locations = LevelUpConfigGlobals.Api.ListLocations(levelUpAccessToken, merchantId);
            }
            catch (LevelUpApiException luEx)
            {
                ShowMessageBox(string.Format("Failed to get locations for merchant with id: {0}!{1}{1}{2}",
                                             merchantId,
                                             Environment.NewLine,
                                             luEx),
                               MessageBoxButton.OK,
                               MessageBoxImage.Error);
            }

            return locations;
        }

        #endregion LevelUp Api Calls

        #region Helpers

        private void DoAuthenticateButtonClick()
        {
            if (string.IsNullOrEmpty(UserNameTextBox.Text) || string.IsNullOrEmpty(PasswordTextBox.Password))
            {
                ShowMessageBox("User name and password are required!");
                return;
            }

            // Clear fields and controls
            MerchantIdValueLabel.Content = string.Empty;
            MerchantNameValueLabel.Content = string.Empty;
            MerchantGroupBox.Visibility = Visibility.Collapsed;
            LocationIdValueLabel.Content = string.Empty;
            LocationAddressValueLabel.Content = string.Empty;
            LocationsGroupBox.Visibility = Visibility.Collapsed;

            //Clear and disable the locations combo box
            Locations.Clear();
            LocationsComboBox.IsEnabled = false;

            //Disable the reload button
            ReloadLocationDataButton.IsEnabled = false;

            //Disable the save button
            SetSaveButtonState(false);

            AuthenticateAsyncDelegate authenticateCallback = new AuthenticateAsyncDelegate(this.Authenticate);

            authenticateCallback.BeginInvoke(UserNameTextBox.Text,
                                             PasswordTextBox.Password,
                                             UpdateFieldsCallback,
                                             authenticateCallback);
        }

        private void UpdateLocationsProperty(IList<Location> locations)
        {
            Locations.Clear();

            foreach (Location location in locations)
            {
                Locations.Add(new LocationViewModel(location));
            }
        }

        private void FillLocationData(int? locationId, string locationAddress = "", string merchantName = "")
        {
            if (locationId.HasValue)
            {
                LocationsGroupBox.Visibility = Visibility.Visible;
                LocationIdValueLabel.Content = locationId;
                LevelUpData.LocationId = locationId;
            }
            else
            {
                LocationsGroupBox.Visibility = Visibility.Collapsed;
                LocationIdValueLabel.Content = string.Empty;
            }

            if (!string.IsNullOrEmpty(locationAddress))
            {
                LocationAddressValueLabel.Visibility = Visibility.Visible;
                LocationAddressLabel.Visibility = Visibility.Visible;
                LocationAddressValueLabel.Content = locationAddress;
            }
            else
            {
                LocationAddressValueLabel.Visibility = Visibility.Collapsed;
                LocationAddressLabel.Visibility = Visibility.Collapsed;
            }

            if (!string.IsNullOrEmpty(merchantName))
            {
                MerchantGroupBox.Visibility = Visibility.Visible;
                MerchantNameLabel.Visibility = Visibility.Visible;
                MerchantNameValueLabel.Visibility = Visibility.Visible;
                MerchantNameValueLabel.Content = merchantName;
                LevelUpData.MerchantName = merchantName;
            }
            else
            {
                MerchantNameValueLabel.Content = string.Empty;
                MerchantNameLabel.Visibility = Visibility.Collapsed;
                MerchantNameValueLabel.Visibility = Visibility.Collapsed;
            }
        }

        private void FillMerchantData(int? merchantId = null, string merchantName = "")
        {
            if (merchantId.HasValue)
            {
                MerchantGroupBox.Visibility = Visibility.Visible;
                MerchantIdValueLabel.Content = merchantId.Value.ToString(CultureInfo.InvariantCulture);

                if (!string.IsNullOrEmpty(merchantName))
                {
                    MerchantNameValueLabel.Content = merchantName;
                }
            }
            else
            {
                MerchantGroupBox.Visibility = Visibility.Collapsed;
                MerchantIdValueLabel.Content = string.Empty;
                MerchantNameValueLabel.Content = string.Empty;
            }
        }

        private void SetSaveButtonState(bool isEnabled)
        {
            if (null != ParentWindow)
            {
                ParentWindow.SaveButton.IsEnabled = isEnabled;
            }
        }

        private void SetStatusLabelText(string text, Brush color)
        {
            if (null != ParentWindow)
            {
                ParentWindow.SetStatusLabelText(text, color);
            }
        }

        private void ShowMessageBox(string message,
                                    MessageBoxButton buttons = MessageBoxButton.OK,
                                    MessageBoxImage icon = MessageBoxImage.Information)
        {
            Helpers.ShowMessageBox(ParentWindow, message, buttons, icon);
        }

        private void ShowMessageBoxOnMainUiThread(string message, MessageBoxButton button, MessageBoxImage image)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal,
                                   new ShowMessageBoxDelegate(ShowMessageBox),
                                   message, button, image);
        }

        private void UpdateFields(string levelUpAccessToken, int? merchantId, bool isMerchant)
        {
            SetStatusLabelText("New Access Token Retrieved", MainWindow.SUCCESS_COLOR);

            if (!isMerchant)
            {
                ShowMessageBox("Could not recognize credentials as belonging to a LevelUp merchant account.");
                return;
            }

            //Clear the text boxes
            UserNameTextBox.Clear();
            PasswordTextBox.Clear();

            //Enable controls
            LocationsComboBox.IsEnabled = true;
            ReloadLocationDataButton.IsEnabled = true;

            //Fill merchant data
            FillMerchantData(merchantId);

            //Get locations async then fill the combo box on callback
            GetLocationsAsyncDelegate getLocationsAsync = new GetLocationsAsyncDelegate(this.GetLocations);

            getLocationsAsync.BeginInvoke(levelUpAccessToken,
                                          merchantId.GetValueOrDefault(0),
                                          FillLocationsComboBoxCallback,
                                          getLocationsAsync);
        }

        #endregion Helpers

        #region Callbacks

        private void FillLocationsComboBoxCallback(IAsyncResult asyncResult)
        {
            GetLocationsAsyncDelegate caller = (GetLocationsAsyncDelegate) asyncResult.AsyncState;

            Dispatcher.Invoke(DispatcherPriority.Normal,
                              new UpdateLocationsPropertyDelegate(UpdateLocationsProperty),
                              caller.EndInvoke(asyncResult));
        }

        private void UpdateFieldsCallback(IAsyncResult asyncResult)
        {
            AuthenticateAsyncDelegate caller = (AuthenticateAsyncDelegate) asyncResult.AsyncState;

            AccessToken levelUpToken = caller.EndInvoke(asyncResult);

            if (levelUpToken != null)
            {
                // always set to false, just before we do save we will check to see whether
                // we should encrypt it
                LevelUpData.SetAccessToken(levelUpToken.Token, false);

                LevelUpData.MerchantId = levelUpToken.MerchantId;

                Dispatcher.Invoke(DispatcherPriority.Normal,
                                  new UpdateFieldsDelegate(UpdateFields),
                                  levelUpToken.Token,
                                  levelUpToken.MerchantId,
                                  levelUpToken.IsMerchant);
            }
        }

        #endregion Callbacks
    }
}