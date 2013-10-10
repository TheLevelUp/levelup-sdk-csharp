using System.Collections.Generic;
using LevelUpApi.Models.Requests;
using LevelUpApi.Models.Responses;

namespace LevelUpApi
{
    /// <summary>
    /// Interface definition for the LevelUp POS integration API
    /// </summary>
    public interface ILevelUpClient
    {
        /// <summary>
        /// Base Url for LevelUp API
        /// </summary>
        string ApiUrlBase { get; }

        /// <summary>
        /// Obtain a LevelUp access token 
        /// </summary>
        /// <param name="clientId">Your LevelUp client ID which LevelUp will have sent to you. 
        /// This is your LevelUp API key</param>
        /// <param name="username">Your LevelUp username</param>
        /// <param name="password">Your LevelUp password</param>
        /// <returns>A LevelUp access token object which includes a .Token member which stores 
        /// the access token you will need to use for subsequent LevelUp calls</returns>
        /// <exception cref="LevelUpApi.LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        AccessToken Authenticate(string clientId, string username, string password);

        /// <summary>
        /// Gets the location details for the specified location id
        /// </summary>
        /// <param name="accessToken">The LevelUp accesstoken obtained from the Authenticate() method</param>
        /// <param name="locationId">Identifies the location for which to return the details</param>
        /// <returns>Detailed location info for the location specified</returns>
        /// <exception cref="LevelUpApi.LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        LocationDetails GetLocationDetails(string accessToken, int locationId);

        /// <summary>
        /// Gets the order details for a given order. This the merchant facing order data so the merchant id is required as well
        /// </summary>
        /// <param name="accessToken">The LevelUp accesstoken obtained from the Authenticate() method</param>
        /// <param name="merchantId">Identifies the merchant on whose behalf you are querying. 
        /// If the access token specified does not belong to the merchant specified, this method will throw an exception</param>
        /// <param name="orderIdentifier">Identifies the order for which to return the details. This should be the order UUID</param>
        /// <returns>Details for the specified order</returns>
        /// <exception cref="LevelUpApi.LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        OrderDetailsResponse GetMerchantOrderDetails(string accessToken, int merchantId, string orderIdentifier);

        /// <summary>
        /// Lists all locations for a specified merchant
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method. 
        /// This will be the .Token member on the returned object</param>
        /// <param name="merchantId">Identifies the merchant for which to return a list of locations</param>
        /// <returns>A list of locations for the specified merchant.</returns>
        /// <exception cref="LevelUpApi.LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        IList<Location> ListLocations(string accessToken, int merchantId);

        /// <summary>
        /// Lists all the orders placed at a specified location
        /// </summary>
        /// <param name="accessToken">The LevelUp accesstoken obtained from the Authenticate() method</param>
        /// <param name="locationId">Identifies the location for which to return the list of orders</param>
        /// <returns>A list of detailed order data for the specified location</returns>
        /// <exception cref="LevelUpApi.LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        IList<OrderDetailsResponse> ListOrders(string accessToken, int locationId);

        /// <summary>
        /// Place an order and pay through LevelUp
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method. 
        /// This will be the .Token member on the returned object</param>
        /// <param name="orderData">An object containing the order data and spend amounts to process through LevelUp</param>
        /// <returns>A response object indicating whether the order was charged successfully and 
        /// the final amount paid including the customer specified tip amount</returns>
        /// <exception cref="LevelUpApi.LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        OrderResponse PlaceOrder(string accessToken, Order orderData);

        /// <summary>
        /// Refund an order placed through LevelUp.
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method. 
        /// This will be the .Token member on the returned object</param>
        /// <param name="orderIdentifier">UUID for the order to refund. 
        /// This information is returned when the order is placed</param>
        /// <param name="managerConfirmation">Some systems require manager confirmation. 
        /// This field should be omitted or set to null if your system does not require manager confirmation</param>
        /// <returns>A response object indicating whether the refund was successfull</returns>
        /// <exception cref="LevelUpApi.LevelUpApiException">Thrown when the order to refund could not be found or 
        /// when the HTTP status returned was not 200 OK</exception>
        RefundResponse RefundOrder(string accessToken,
                                   string orderIdentifier,
                                   string managerConfirmation = null);
    }
}
