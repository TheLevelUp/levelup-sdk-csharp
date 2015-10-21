using System.Windows.Media;
using LevelUp.Api.Client.Models.Requests;
using LevelUp.Api.Client.Models.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace LevelUpExampleApp
{
    public partial class TestTabItem : TabItem
    {
        private Order _levelUpOrder;
        private string _lastOrderUuid;
        private MainWindow _parentWin = null;
        const int SPEND_AMOUNT_CENTS = 10;

        private MainWindow ParentWindow
        {
            get { return _parentWin ?? (_parentWin = Window.GetWindow(this) as MainWindow); }
        }

        private readonly List<Item> _orderItems = new List<Item>
            {
                new Item("Baklava", "A tasty Turkish treat", "9999", "123456789", "Pastry", 5, 5),
                new Item("Meatball Sub", "A seriously saucy spuckie", "2468", "987654321", "Hoagies", 2, 2, 1),
                new Item("Coouuuuukie Crisp", "Sugary breakfast dessert", "13579", "11111", "Cereal", 3, 1, 3),
            };

        #region Delegates

        private delegate void ShowMessageBoxDelegate(string message, MessageBoxButton button, MessageBoxImage image);

        #endregion Delegates

        public TestTabItem()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                };

            InitializeComponent();
        }

        #region UI Event Handlers

        private void CreateOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (!LoadLevelUpData())
            {
                SetStatusLabelText("Please Configure LevelUp Data", LevelUpExampleAppGlobals.ERROR_COLOR);
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
            OrderDetailsResponse response =
                LevelUpExampleAppGlobals.Api.GetMerchantOrderDetails(LevelUpData.Instance.AccessToken,
                                                                     LevelUpData.Instance.MerchantId.GetValueOrDefault(0),
                                                                     _lastOrderUuid);

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

            OrderResponse response = LevelUpExampleAppGlobals.Api.PlaceOrder(LevelUpData.Instance.AccessToken,
                                                                             _levelUpOrder);
            _lastOrderUuid = response.Identifier;

            ResponseTextBox.Text = JsonConvert.SerializeObject(response);
            RefundLastOrderButton.IsEnabled = true;
            GetLastOrderDetailsButton.IsEnabled = true;
            PlaceOrderButton.IsEnabled = false;
        }

        private void OnQrTextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            if (!QrDataTextBox.Text.ToUpperInvariant().StartsWith("LU"))
            {
                QrDataTextBox.Clear();
            }
        }

        private void OnQrTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(QrDataTextBox.Text))
            {
                QrDataTextBox.Text = "Enter QR code data here";
            }
        }

        private void OnQrTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (null != CreateOrderButton)
            {
                CreateOrderButton.IsEnabled = !string.IsNullOrEmpty(QrDataTextBox.Text);
            }
        }

        private void RefundButton_Click(object sender, RoutedEventArgs e)
        {
            RefundResponse response = LevelUpExampleAppGlobals.Api.RefundOrder(LevelUpData.Instance.AccessToken,
                                                                               _lastOrderUuid);

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

            var orders = LevelUpExampleAppGlobals.Api.ListOrders(LevelUpData.Instance.AccessToken,
                                                                 LevelUpData.Instance.LocationId.GetValueOrDefault(0));

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
            string errorMsg;
            if (!LevelUpData.Instance.IsValid(out errorMsg))
            {
                LevelUpExampleAppGlobals.LoadConfigFile();

                if (null == LevelUpData.Instance)
                {
                    ShowMessageBox("Please save LevelUp configuration data first!",
                                   MessageBoxButton.OK,
                                   MessageBoxImage.Exclamation);
                    return false;
                }
            }

            return LevelUpData.Instance.IsValid();
        }

        private void ShowMessageBox(string message,
                            MessageBoxButton buttons = MessageBoxButton.OK,
                            MessageBoxImage icon = MessageBoxImage.Information)
        {
            MessageBox.Show(ParentWindow, message, ". : LevelUp Example App", buttons, icon);
        }

        private void SetStatusLabelText(string text, Brush color)
        {
            if (null != ParentWindow)
            {
                ParentWindow.SetStatusLabelText(text, color);
            }
        }

        #endregion Helper Methods
    }
}
