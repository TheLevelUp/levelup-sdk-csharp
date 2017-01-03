using System;
using System.Collections.Generic;
using System.Linq;
using LevelUp.Api.Client;
using LevelUp.Api.Client.ClientInterfaces;
using LevelUp.Api.Client.Models.Requests;
using LevelUp.Api.Client.Models.Responses;
using LevelUp.Api.Http;

namespace ExampleApp
{
    class Program
    {
        private static void PrintUsageString()
        {
            Console.WriteLine("usage: ExampleApp.exe \"merchant_user_name\" \"merchant_password\" \"api_key\" " +
                                      "\"location_id\" \"customers_qr_code\"");
        }

        /// <summary>
        /// This is an example console application that walks through how one might use the LevelUp C# SDK
        /// to place an order in the LevelUp sandbox platform.
        /// </summary>
        static void Main(string[] args)
        {
            #region Parse Arguments

            if (args.Length != 5)
            {
                PrintUsageString();
                return;
            }

            int locationId;
            if (!Int32.TryParse(args[3], out locationId))
            {
                PrintUsageString();
                return;
            }

            string merchantUserName = args[0];
            string merchantPassword = args[1];
            string apiKey = args[2];
            string customersQrCode = args[4];

            #endregion

            // Step 1: Authenticate
            string accessToken = Authenticate(merchantUserName, merchantPassword, apiKey);

            // Step 2: Add items to your order
            List<Item> itemsToOrder = CreateAFewExampleItems();

            // Step 3: Propose the order to the LevelUp platform.  Find out how much discount credit to apply
            ProposedOrderResponse proposedOrder = ProposedOrder(accessToken, locationId, customersQrCode, itemsToOrder);

            // Step 4: Apply the discount credit on your POS
            ApplyDiscountToPOS(proposedOrder);

            // Step 5: Figure out the tax on your POS
            int taxInCents = CalculateTaxOnCheck(itemsToOrder, proposedOrder.DiscountAmountCents);

            // Step 6: Complete the order with the LevelUp platform.  The consumer's account is charged.
            CompletedOrderResponse completedOrder = CompleteOrder(accessToken, locationId, customersQrCode, 
                proposedOrder.ProposedOrderIdentifier, proposedOrder.DiscountAmountCents, taxInCents, itemsToOrder);

            // Step 7: Apply the LevelUp payment & giftcard amount on your POS
            ApplyPayment(completedOrder);

            // Step 8: (cleanup) Refund the order
            RefundOrder(accessToken, completedOrder.OrderIdentifier);

            Console.WriteLine("Order Complete!  Check out the code to see how it's done.");
        }


        #region Step 1: Authenticate

        /// <summary>
        /// The LevelUpEnviornment defines which LevelUp platform you wish to target.  You may specify "Sandbox", 
        /// "Staging", "Production", or you may specify a custom url via the constructor.  We suggest doing your 
        /// testing in the "Sandbox" environment or in a stubbed testing environment of your own. Any transactions in 
        /// the Sandbox environment will forgo the transfer of real money.
        /// </summary>
        private static readonly LevelUpEnvironment LuEnvironment = LevelUpEnvironment.Sandbox;

        /// <summary>
        /// The agent identifier is a descriptive tag that will be attached to any API request.  The provided data is 
        /// functionally immaterial, however these details are recorded in logs on the backend and may help to 
        /// identify/diagnose issues with a request.
        /// </summary>
        /// <remarks>
        /// It's up to the client how they would like to populate this data.  A pertinent suggestion here might be to
        /// grab the CompanyName, etc. in your AssemblyInfo.cs file.
        /// </remarks>
        private static readonly AgentIdentifier LuIdentifier = new AgentIdentifier("myCompanyName, LLC", "Example App",
            "1.0.0", "Win10, .NET 4.5");

        /// <summary>
        /// Performs the Authenticate(...) call to retrieve the access token that will be required for most of the 
        /// subsequent requests you make to the LevelUp platform.
        /// </summary>
        private static string Authenticate(string merchantUserName, string merchantPassword, string apiKey)
        {
            try
            {
                IAuthenticate authenticator = LevelUpClientFactory.Create<IAuthenticate>(LuIdentifier, LuEnvironment);

                AccessToken tokenObj = authenticator.Authenticate(apiKey, merchantUserName, merchantPassword);
                                                                    
                return tokenObj.Token;
            }
            catch (LevelUpApiException ex)
            {
                Console.WriteLine(string.Format("There was an error authenticating your user.  Are the credentials " +
                                              "correct? {0}{0}Exception Details: {1}", Environment.NewLine, ex.Message));
                return null;
            }
        }

        #endregion

        #region Step 2: Create A Few Example Items

        private static readonly System.Func<Item> exampleStandardItem = () =>
            new Item("Clam Chowder",
                     "Creamy chowder made with all natural clams and served with crackers.",
                     "POT-JQ-PL-36-GG-C50",
                     "043200005264",
                     "Entree",
                     150,    // Charged price is (unit price * quantity) - (non-LevelUp discount)
                     150,    // Standard price is (unit price * quantity)
                     1,
                     new List<Item>(new Item[] { exampleModifierItem() })
                );

        private static readonly System.Func<Item> exampleModifierItem = () =>
            new Item("Extra oyster crackers", "", "SAL-RR-51-QB-Z20", "027303074444", "Modifiers", 5, 5);

        // Some items may be exempt from earning loyalty or may not be paid for with discount 
        // credit (ex. alcohol, in some U.S. states)
        private static readonly System.Func<Item> exampleExemptItem = () =>
            new Item("Sam Adam's Lager, 1 pint",
                     "A mellow beer made right in the heart of Boston",
                     "BER-JQ-RL-36-AA-Q50",
                     "023400075361",
                     "Alcohol",
                     500,
                     500,
                     1,
                     null
                );

        private static List<Item> CreateAFewExampleItems()
        {
            return new List<Item>
            {
                exampleStandardItem(),
                exampleExemptItem(),
                exampleExemptItem()
            };
        }

        #endregion

        #region Step 3: Propose Order
        
        /// <summary>
        /// Placing an order with LevelUp is a two-step process.  The first step is to "propose" an order 
        /// with the LevelUp platform. This is an api call that registers your intent to place an order, and 
        /// allows our platform to determine if there is any available discount that may be applied to the 
        /// check.  The response from this call will tell the POS how much discount to apply, at which point 
        /// the POS may apply the discount, recalculate the tax, and complete the order.
        /// </summary>
        private static ProposedOrderResponse ProposedOrder(string accessToken, int locationId, string qrCode, List<Item> itemsToOrder)
        {
            try
            {
                var orderProcessor = LevelUpClientFactory.Create<IManageProposedOrders>(LuIdentifier, LuEnvironment);

                return orderProcessor.CreateProposedOrder(
                    accessToken: accessToken, 
                    locationId: locationId,                 // The location id associated with the store in which the order was made.
                    qrPaymentData: qrCode,                  // The QR code scanned from a customer's phone
                    spendAmountCents: GetSpendAmountInCents(itemsToOrder), 
                    taxAmountCents: CalculateTaxInCents(GetSpendAmountInCents(itemsToOrder)), 
                    exemptionAmountCents: GetExemptAmountInCents(itemsToOrder), 
                    register: "Register 2", 
                    cashier: "Jane", 
                    identifierFromMerchant: "123456",       // If your POS has it's own system of associating orders with an ID, put that ID here.
                    receiptMessageHtml: "Thanks for eating at <strong>Freddy's Clamshack</strong>",      //  Added to the LevelUp email recipt.
                    items: itemsToOrder);
            }
            catch (LevelUpApiException ex)
            {
                Console.WriteLine(string.Format("There was an error placing the order! {0}{0}" +
                                             "Exception Details: {1}", Environment.NewLine, ex.Message));
                return null;
            }
        }

        #endregion

        #region Step 4: Apply Discount

        private static void ApplyDiscountToPOS(ProposedOrderResponse proposedOrderResponse)
        {
            Console.WriteLine("Discount to apply: " + toDollarString(proposedOrderResponse.DiscountAmountCents));
            // Here is where you would submit the discount to your POS system.
        }

        #endregion

        #region Step 5: Calculate Tax

        private static int CalculateTaxOnCheck(List<Item> itemsOnCheck, int discountYouAlreadyAppliedInCents)
        {
            return CalculateTaxInCents(GetSpendAmountInCents(itemsOnCheck) - discountYouAlreadyAppliedInCents);
        }

        #endregion

        #region Step 6: Complete Proposed Order

        private static CompletedOrderResponse CompleteOrder(string accessToken, int locationId, string qrCode, 
            string proposedOrderUUID, int discountAppliedInCents, int taxAmountInCents, List<Item> itemsToOrder)
        { 
            try
            {
                var orderProcessor = LevelUpClientFactory.Create<IManageProposedOrders>(LuIdentifier, LuEnvironment);

                return orderProcessor.CompleteProposedOrder(
                    accessToken: accessToken, 
                    locationId: locationId, 
                    qrPaymentData: qrCode, 
                    proposedOrderUuid: proposedOrderUUID, 
                    spendAmountCents: GetSpendAmountInCents(itemsToOrder), 
                    taxAmountCents: taxAmountInCents, 
                    exemptionAmountCents: GetExemptAmountInCents(itemsToOrder), 
                    appliedDiscountAmountCents: discountAppliedInCents, 
                    register: "Register 2", 
                    cashier: "Jane", 
                    identifierFromMerchant: "123456", 
                    receiptMessageHtml: "Thanks for eating at <strong>Freddy's Clamshack</strong>", 
                    items: itemsToOrder);
            }
            catch (LevelUpApiException ex)
            {
                Console.WriteLine("There was an error completing the order! {0}{0}" +
                                  "Exception Details: {1}", Environment.NewLine, ex.Message);

                // Following the proposed order, you may have taken some action to register the
                // discount amount with your POS (step 4.)  Since the complete order call failed,
                // you will likely need to roll-back that discount you applied to the check here.

                return null;
            }
        }

        #endregion

        #region Step 7: Apply Payment

        private static void ApplyPayment(CompletedOrderResponse response)
        {
            // Here is where you would submit the Levelup payment to your POS system.
        }

        #endregion

        #region Step 8: Refund Order

        private static void RefundOrder(string accessToken, string orderId)
        {
            try
            {
                var refundProcessor = LevelUpClientFactory.Create<ICreateRefund>(LuIdentifier, LuEnvironment);
                refundProcessor.RefundOrder(accessToken, orderId);
            }
            catch (LevelUpApiException ex)
            {
                Console.WriteLine(string.Format("There was an error refunding the order! {0}{0}" +
                                          "Exception Details: {1}", Environment.NewLine, ex.Message));
            }
        }

        #endregion

        private static System.Func<int, string> toDollarString = (x) => string.Format("$ {0:0.00}", x / 100 );

        private static int GetSpendAmountInCents(List<Item> items)
        {
            return items.Aggregate(0, (sum, next) => sum + next.ChargedPrice);
        }

        private static int GetExemptAmountInCents(List<Item> items)
        {
            // Some items may be exempt from earning loyalty or may not be paid for with discount credit (ex. alcohol, 
            // in some U.S. states)
            return items.Where(x => x.Category.Contains("Alcohol"))
                        .Aggregate(0, (sum, next) => sum + next.StandardPrice.GetValueOrDefault(0));
        }

        private static int CalculateTaxInCents(int totalSpendAmount)
        {
            // Our simple tax scheme charges 5% on everything.  Yours will likely be more refined.
            return (int)Math.Floor(0.05 * totalSpendAmount);
        }
    }
}
