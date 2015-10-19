using LevelUp.Api.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using LevelUp.Api.Client.Models.Responses;
using LevelUp.Api.Http;

namespace ConfigurationTool
{
    /// <summary>
    /// ViewModel for MVVM
    /// </summary>
    public partial class LevelUpTabItem : TabItem
    {
        private const string CLIENT_ID = "LevelUp API key goes here";  //TODO: Set your API key here
        private ILevelUpClient _api = null;
        private MainWindow _parentWin = null;
        private readonly Color SUCCESS_COLOR = Colors.LimeGreen;
        private readonly Color ERROR_COLOR = Colors.IndianRed;
        private const string PLEASE_AUTHENTICATE = "Please Authenticate";

        private ILevelUpClient Api
        {
            get { return _api ?? LevelUpClientFactory.Create("LevelUp", "ConfigurationApp", "1.0.0.0", ".NET 3.0"); }
        }

        #region Delegates

        private delegate AccessToken AuthenticateAsyncDelegate(string username, string password);

        private delegate void FillLocationsComboBoxDelegate(IList<Location> locations);

        private delegate IList<Location> GetLocationsAsyncDelgate(string levelUpAccessToken, int merchantId);

        private delegate void ShowMessageBoxDelegate(string message, MessageBoxButton button, MessageBoxImage image);

        private delegate void UpdateFieldsDelegate(string levelUpAccessToken, int? merchantId, bool isMerchant);

        #endregion Delegates

        public LevelUpTabItem()
        {
            InitializeComponent();
        }

        #region UI Event Handlers

        private void AuthenticateButton_Click(object sender, RoutedEventArgs e)
        {
            DoAuthenticateButtonClick();
        }

        private void OnControlLoaded(object sender, RoutedEventArgs e)
        {
            _parentWin = Window.GetWindow(this) as MainWindow;

            ConfiguredSettings settings = ConfiguredSettings.LoadConfigData();

            if (settings != null)
            {
                LevelUpData.Instance.AccessToken = settings.LevelUpAccessToken;
                LevelUpData.Instance.MerchantId = settings.LevelUpMerchantId;
                LevelUpData.Instance.LocationId = settings.LevelUpLocationId;
                LevelUpData.Instance.MerchantName = settings.LevelUpMerchantName;

                if (!string.IsNullOrEmpty(LevelUpData.Instance.AccessToken))
                {
                    StatusLabel.Foreground = new SolidColorBrush(SUCCESS_COLOR);
                    StatusLabel.Content = "Access Token Retrieved";
                }

                FillMerchantData(LevelUpData.Instance.MerchantId);

                var locations = GetLocations(LevelUpData.Instance.AccessToken,
                                             LevelUpData.Instance.MerchantId.GetValueOrDefault(0));

                FillLocationsComboBox(locations, LevelUpData.Instance.LocationId.GetValueOrDefault(0));

                LocationDetails details = GetLocationDetails(LevelUpData.Instance.AccessToken,
                                                             LevelUpData.Instance.LocationId.GetValueOrDefault(0));

                if (null == details)
                {
                    FillLocationData(LevelUpData.Instance.LocationId);
                }
                else
                {
                    FillLocationData(details.LocationId, details.Address.ToString(), details.MerchantName);
                }

                //Enable controls
                LocationsComboBox.IsEnabled = true;
                ReloadLocationDataButton.IsEnabled = true;
                SaveButton.IsEnabled = true;
            }
            else
            {
                StatusLabel.Foreground = new SolidColorBrush(ERROR_COLOR);
                StatusLabel.Content = PLEASE_AUTHENTICATE;
            }
        }

        private void OnLocationSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Fill merchant name label & location info
            string selectedLocationId = LocationsComboBox.SelectedValue as string;
            int locationId;

            if (string.IsNullOrEmpty(selectedLocationId))
            {
                return;
            }

            if (!int.TryParse(selectedLocationId, out locationId))
            {
                throw new Exception(string.Format("\"{0}\" is not a valid LevelUp location id.", selectedLocationId));
            }

            LocationDetails details = GetLocationDetails(LevelUpData.Instance.AccessToken, locationId);

            if (null != details)
            {
                FillLocationData(details.LocationId, details.Address.ToString(), details.MerchantName);
            }
            else
            {
                FillLocationData(locationId);
            }

            SaveButton.IsEnabled = true;
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
            if (null == LevelUpData.Instance || string.IsNullOrEmpty(LevelUpData.Instance.AccessToken))
            {
                ShowMessageBox("Please authenticate!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            //Clear the content
            LocationIdValueLabel.Content = string.Empty;
            LocationAddressValueLabel.Content = string.Empty;

            //Disable the save button
            SaveButton.IsEnabled = false;

            //Get locations async then fill the combo box on callback
            GetLocationsAsyncDelgate getLocationsAsync = new GetLocationsAsyncDelgate(this.GetLocations);

            IAsyncResult result = getLocationsAsync.BeginInvoke(LevelUpData.Instance.AccessToken,
                                                                LevelUpData.Instance.MerchantId.GetValueOrDefault(0),
                                                                new AsyncCallback(FillLocationsComboBoxCallback),
                                                                getLocationsAsync);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ConfiguredSettings settingsToSave = new ConfiguredSettings(LevelUpData.Instance.AccessToken,
                                                                       LevelUpData.Instance.MerchantId);

            if (null == LocationsComboBox.SelectedValue)
            {
                ShowMessageBox("Please select a location.");
                return;
            }

            int locationIdInt;
            if (!int.TryParse(LocationsComboBox.SelectedValue.ToString(), out locationIdInt))
            {
                ShowMessageBox(string.Format("\"{0}\" is not a valid location id.", LocationsComboBox.SelectedValue),
                               MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            settingsToSave.LevelUpLocationId = locationIdInt;

            //Verify all fields have values
            string messages = settingsToSave.Verify();

            if (messages.Length > 0)
            {
                ShowMessageBox(messages, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                //Save to specified format
                if (XmlFormatRadioButton.IsChecked.HasValue && XmlFormatRadioButton.IsChecked.Value)
                {
                    settingsToSave.SerializeToXml();
                }
                else
                {
                    settingsToSave.SerializeToJson();
                }

                StatusLabel.Foreground = new SolidColorBrush(SUCCESS_COLOR);
                StatusLabel.Content = "Configuration Data Saved";
            }
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
                token = Api.Authenticate(CLIENT_ID, username, password);
            }
            catch (LevelUpApiException luEx)
            {
                ShowMessageBoxOnMainUiThread(
                    string.Format("LevelUp Authentication failed!{0}{0}{1}", Environment.NewLine, luEx.Message),
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return token;
        }

        private LocationDetails GetLocationDetails(string levelUpAccessToken, int locationId)
        {
            LocationDetails details = null;

            try
            {
                details = Api.GetLocationDetails(levelUpAccessToken, locationId);
            }
            catch (LevelUpApiException luEx)
            {
                ShowMessageBox(luEx.Message, MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return details;
        }

        private IList<Location> GetLocations(string levelUpAccessToken, int merchantId)
        {
            IList<Location> locations = null;

            try
            {
                locations = Api.ListLocations(levelUpAccessToken, merchantId);
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

            AuthenticateAsyncDelegate authenticateCallback = new AuthenticateAsyncDelegate(this.Authenticate);

            IAsyncResult result = authenticateCallback.BeginInvoke(UserNameTextBox.Text,
                                                                   PasswordTextBox.Password,
                                                                   new AsyncCallback(UpdateFieldsCallback),
                                                                   authenticateCallback);

            // Clear fields and controls
            MerchantIdValueLabel.Content = string.Empty;
            MerchantNameValueLabel.Content = string.Empty;
            LocationIdValueLabel.Content = string.Empty;
            LocationAddressValueLabel.Content = string.Empty;

            //Clear and disable the locations combo box
            LocationsComboBox.ItemsSource = null;
            LocationsComboBox.Items.Clear();
            LocationsComboBox.IsEnabled = false;

            //Disable the reload button
            ReloadLocationDataButton.IsEnabled = false;

            //Disable the save button
            SaveButton.IsEnabled = false;
        }

        private void FillLocationsComboBox(IList<Location> locations)
        {
            FillLocationsComboBox(locations, null);
        }

        private void FillLocationsComboBox(IList<Location> locations, int? locationToSelect)
        {
            if (null == locations || locations.Count == 0)
            {
                return;
            }

            DataTable dt = new DataTable("LocationsTable");
            dt.Columns.Add("Id");
            dt.Columns.Add("DisplayName");
            DataRow dr = null;
            foreach (Location loc in locations)
            {
                dr = dt.NewRow();
                dr["Id"] = loc.LocationId;
                string displayName = string.IsNullOrEmpty(loc.Name) ? string.Empty : " : " + loc.Name;
                dr["DisplayName"] = string.Format("{0}{1}", loc.LocationId, displayName);
                dt.Rows.Add(dr);
            }

            LocationsComboBox.ItemsSource = null;
            LocationsComboBox.Items.Clear();
            LocationsComboBox.ItemsSource = ((IListSource)dt).GetList();
            LocationsComboBox.DisplayMemberPath = "DisplayName";
            LocationsComboBox.SelectedValuePath = "Id";

            if (locationToSelect.HasValue && locationToSelect.Value > 0)
            {
                LocationsComboBox.SelectedValue = locationToSelect.Value;
            }
        }

        private void FillLocationData(int? locationId, string locationAddress = "", string merchantName = "")
        {
            if (locationId.HasValue)
            {
                LocationIdValueLabel.Content = locationId;
                LevelUpData.Instance.LocationId = locationId;
            }

            if (!string.IsNullOrEmpty(locationAddress))
            {
                LocationAddressValueLabel.Content = locationAddress;
            }

            if (!string.IsNullOrEmpty(merchantName))
            {
                MerchantNameValueLabel.Content = merchantName;
                LevelUpData.Instance.MerchantName = merchantName;
            }
        }

        private void FillMerchantData(int? merchantId = null, string merchantName = "")
        {
            MerchantIdValueLabel.Content = merchantId.HasValue ? merchantId.Value.ToString() : string.Empty;
            MerchantNameValueLabel.Content = merchantName ?? string.Empty;
        }

        private void ShowMessageBox(string message,
                                    MessageBoxButton buttons = MessageBoxButton.OK,
                                    MessageBoxImage icon = MessageBoxImage.Information)
        {
            MessageBox.Show(_parentWin, message, ". : LevelUp Configuration Tool", buttons, icon);
        }

        private void ShowMessageBoxOnMainUiThread(string message, MessageBoxButton button, MessageBoxImage image)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal,
                                   new ShowMessageBoxDelegate(ShowMessageBox),
                                   message, button, image);
        }

        private void UpdateFields(string levelUpAccessToken, int? merchantId, bool isMerchant)
        {
            StatusLabel.Foreground = new SolidColorBrush(SUCCESS_COLOR);
            StatusLabel.Content = "New Access Token Retrieved";

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
            GetLocationsAsyncDelgate getLocationsAsync = new GetLocationsAsyncDelgate(this.GetLocations);

            IAsyncResult result = getLocationsAsync.BeginInvoke(levelUpAccessToken,
                                                                   merchantId.GetValueOrDefault(0),
                                                                   new AsyncCallback(FillLocationsComboBoxCallback),
                                                                   getLocationsAsync);
        }

        #endregion Helpers

        #region Callbacks

        private void FillLocationsComboBoxCallback(IAsyncResult asyncResult)
        {
            GetLocationsAsyncDelgate caller = (GetLocationsAsyncDelgate)asyncResult.AsyncState;

            IList<Location> locations = caller.EndInvoke(asyncResult);

            this.Dispatcher.Invoke(DispatcherPriority.Normal,
                                   new FillLocationsComboBoxDelegate(FillLocationsComboBox),
                                   locations);
        }

        private void UpdateFieldsCallback(IAsyncResult asyncResult)
        {
            AuthenticateAsyncDelegate caller = (AuthenticateAsyncDelegate)asyncResult.AsyncState;

            AccessToken levelUpToken = caller.EndInvoke(asyncResult);

            if (levelUpToken != null)
            {
                LevelUpData.Instance.AccessToken = levelUpToken.Token;
                LevelUpData.Instance.MerchantId = levelUpToken.MerchantId;

                this.Dispatcher.Invoke(DispatcherPriority.Normal,
                                       new UpdateFieldsDelegate(UpdateFields),
                                       levelUpToken.Token,
                                       levelUpToken.MerchantId,
                                       levelUpToken.IsMerchant);
            }
        }

        #endregion Callbacks
    }
}
