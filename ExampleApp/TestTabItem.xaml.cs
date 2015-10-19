using LevelUp.Api.Client;
using LevelUp.Api.Client.Models.Requests;
using LevelUp.Api.Client.Models.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ConfigurationTool
{
    public partial class TestTabItem : TabItem
    {
        private ILevelUpClient _api = null;
        private Order _levelUpOrder;
        private string _lastOrderUUID;
        private MainWindow _parentWin = null;
        const int SPEND_AMOUNT_CENTS = 10;

        private readonly List<Item> _orderItems = new List<Item>
            {
                new Item("Baklava", "A tasty treat", "9999", "123456789", "Pastry", 5, 5),
                new Item("Meatball Sub", "A seriously saucy spuckie", "2468", "987654321", "Hoagies", 2, 2, 1),
                new Item("Coouuuuukie Crisp", "Sugary breakfast dessert", "13579", "11111", "Cereal", 3, 1, 3),
            };

        #region Delegates

        private delegate void ShowMessageBoxDelegate(string message, MessageBoxButton button, MessageBoxImage image);

        #endregion Delegates

        private ILevelUpClient Api
        {
            get { return _api ?? LevelUpClientFactory.Create("LevelUp", "TestApp", "0.1.0.0", ".NET 3.0"); }
        }

        public TestTabItem()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
            };

            InitializeComponent();
        }

        #region UI Event Handlers

        private void OnControlLoaded(object sender, RoutedEventArgs e)
        {
            _parentWin = Window.GetWindow(this) as MainWindow;
        }

        private void CreateOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (!LoadLevelUpData())
            {
                return;
            }

            PlaceOrderButton.IsEnabled = true;

            _levelUpOrder = new Order(LevelUpData.Instance.LocationId.GetValueOrDefault(0),
                          QrDataTextBox.Text.Trim(),
                          SPEND_AMOUNT_CENTS,
                          null,
                          "Seth",
                          "1234",
                          _orderItems);

            OrderContentTextBox.Text = JsonConvert.SerializeObject(_levelUpOrder);
        }

        private void GetOrderDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            OrderDetailsResponse response = Api.GetMerchantOrderDetails(LevelUpData.Instance.AccessToken,
                                                                        LevelUpData.Instance.MerchantId.GetValueOrDefault(0), 
                                                                        _lastOrderUUID);

            ResponseTextBox.Clear();
            ResponseTextBox.Text = JsonConvert.SerializeObject(response);
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            ResponseTextBox.Clear();

            try
            {
                _levelUpOrder = JsonConvert.DeserializeObject<Order>(OrderContentTextBox.Text);

            }
            catch (Exception)
            {
                ShowMessageBox("Order format is not valid!\n\nCannot place order",
                               MessageBoxButton.OK,
                               MessageBoxImage.Error);
                OrderContentTextBox.Clear();
                PlaceOrderButton.IsEnabled = false;
                return;
            }
            
            OrderResponse response = Api.PlaceOrder(LevelUpData.Instance.AccessToken, _levelUpOrder);
            _lastOrderUUID = response.Identifier;

            ResponseTextBox.Text = JsonConvert.SerializeObject(response);
            RefundLastOrderButton.IsEnabled = true;
            GetLastOrderDetailsButton.IsEnabled = true;
            PlaceOrderButton.IsEnabled = false;
        }

        private void OnQrTextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            if (!QrDataTextBox.Text.StartsWith("LU"))
            {
                QrDataTextBox.Clear();
            }
        }

        private void OnQrTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(QrDataTextBox.Text))
            {
                CreateOrderButton.IsEnabled = true;
            }
            else
            {
                QrDataTextBox.Text = "Enter QR code data here";
            }
        }

        private void RefundButton_Click(object sender, RoutedEventArgs e)
        {
            RefundResponse response = Api.RefundOrder(LevelUpData.Instance.AccessToken, _lastOrderUUID);

            ResponseTextBox.Clear();
            ResponseTextBox.Text = JsonConvert.SerializeObject(response);

            RefundLastOrderButton.IsEnabled = false;
            PlaceOrderButton.IsEnabled = true;
        }

        private void ShowRecentOrdersButton_Click(object sender, RoutedEventArgs e)
        {
            if (!LoadLevelUpData())
            {
                return;
            }

            var orders = Api.ListOrders(LevelUpData.Instance.AccessToken, LevelUpData.Instance.LocationId.GetValueOrDefault(0));

            StringBuilder sb = new StringBuilder();
            int limit = 10;

            for (int i = 0; i < limit; i++)
            {
                if (i <= orders.Count && orders[i] != null)
                {
                    sb.AppendLine(JsonConvert.SerializeObject(orders[i]));
                }
            }

            ResponseTextBox.Clear();
            ResponseTextBox.Text = sb.ToString();
        }

        #endregion UI Event Handlers

        #region Helper Methods

        private bool LoadLevelUpData()
        {
            if (!LevelUpData.Instance.IsValid)
            {
                ConfiguredSettings settings = ConfiguredSettings.LoadConfigData();

                if (null == settings)
                {
                    ShowMessageBox("Please save LevelUp configuration data first!",
                                   MessageBoxButton.OK,
                                   MessageBoxImage.Exclamation);
                    return false;
                }

                LevelUpData.Instance.AccessToken = settings.LevelUpAccessToken;
                LevelUpData.Instance.MerchantId = settings.LevelUpMerchantId;
                LevelUpData.Instance.LocationId = settings.LevelUpLocationId;
                LevelUpData.Instance.MerchantName = settings.LevelUpMerchantName;
            }

            return LevelUpData.Instance.IsValid;
        }

        private void ShowMessageBox(string message,
                            MessageBoxButton buttons = MessageBoxButton.OK,
                            MessageBoxImage icon = MessageBoxImage.Information)
        {
            MessageBox.Show(_parentWin, message, ".: LevelUp Configuration Tool", buttons, icon);
        }

        #endregion Helper Methods
    }
}
