using LevelUpApi.Models.Requests;
using LevelUpApi.Models.Responses;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Net;

namespace LevelUpApi
{
    public class LevelUpApiV14 : ILevelUpClient
    {
        private const string API_URL_BASE = "https://api.thelevelup.com/v14/";

        #region Constructors
        
        internal LevelUpApiV14(AgentIdentifier identifier)
        {
            this.Identifier = identifier;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Base URL for this version of the LevelUp API
        /// </summary>
        public string ApiUrlBase { get { return API_URL_BASE; } }

        /// <summary>
        /// Identifying information for the group or individual developing this software
        /// </summary>
        public AgentIdentifier Identifier { get; set; }

        #endregion Properties

        #region Public Methods

        #region ILevelUpApi

        /// <summary>
        /// Obtain a LevelUp access token 
        /// </summary>
        /// <param name="clientId">Your LevelUp client ID which LevelUp will have sent to you. 
        /// This is your API key</param>
        /// <param name="username">Your LevelUp username</param>
        /// <param name="password">Your LevelUp password</param>
        /// <returns>A LevelUp access token object which includes a .Token member which stores 
        /// the access token you will need to use for subsequent LevelUp calls</returns>
        /// <exception cref="LevelUpApi.LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        public AccessToken Authenticate(string clientId, string username, string password)
        {
            string accessTokenUri = HttpRest.CreateCombinedUrl(API_URL_BASE, "access_tokens");

            AccessTokenRequest luTokenRequest = new AccessTokenRequest(clientId, username, password);

            string body = JsonConvert.SerializeObject(luTokenRequest);

            IRestResponse response = LevelUpRestWrapper.Post(accessTokenUri, body, string.Empty, Identifier.ToString());

            ThrowExceptionOnBadResponseCode(response);

            return JsonConvert.DeserializeObject<AccessToken>(response.Content);
        }

        /// <summary>
        /// Gets the location details for the specified location id
        /// </summary>
        /// <param name="accessToken">The LevelUp accesstoken obtained from the Authenticate() method</param>
        /// <param name="locationId">Identifies the location for which to return the details</param>
        /// <returns>Detailed location info for the location specified</returns>
        /// <exception cref="LevelUpApi.LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        public LocationDetails GetLocationDetails(string accessToken, int locationId)
        {
            string locationDetailsUri = HttpRest.CreateCombinedUrl(API_URL_BASE,
                                                                   string.Format("locations/{0}", locationId));

            IRestResponse response = LevelUpRestWrapper.Get(locationDetailsUri, accessToken, Identifier.ToString());

            // Special case 404 error since this means the location does not exist, is not visible, 
            // or the merchant owner of the location is not live
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new LevelUpApiException(
                    string.Format("Cannot get location details for location {0}. " +
                                  "This location may not exist, not be visible, or the merchant owner may not be live.",
                                  locationId), response.StatusCode, response.ErrorException);
            }

            ThrowExceptionOnBadResponseCode(response);

            return JsonConvert.DeserializeObject<LocationDetails>(response.Content);
        }

        /// <summary>
        /// Gets the order details for a given order. This the merchant facing order data so the merchant id is required as well
        /// </summary>
        /// <param name="accessToken">The LevelUp accesstoken obtained from the Authenticate() method</param>
        /// <param name="merchantId">Identifies the merchant on whose behalf you are querying. 
        /// If the access token specified does not belong to the merchant specified, this method will throw an exception</param>
        /// <param name="orderIdentifier">Identifies the order for which to return the details. This should be the order UUID</param>
        /// <returns>Details for the specified order</returns>
        /// <exception cref="LevelUpApi.LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        public OrderDetailsResponse GetMerchantOrderDetails(string accessToken, int merchantId, string orderIdentifier)
        {
            string orderDetailsUri = HttpRest.CreateCombinedUrl(API_URL_BASE,
                                                                string.Format("merchants/{0}/orders/{1}", merchantId,
                                                                              orderIdentifier));

            IRestResponse response = LevelUpRestWrapper.Get(orderDetailsUri, accessToken, Identifier.ToString());

            ThrowExceptionOnBadResponseCode(response);

            return JsonConvert.DeserializeObject<OrderDetailsResponse>(response.Content);
        }

        /// <summary>
        /// Lists all locations for a specified merchant
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method. 
        /// This will be the .Token member on the returned object</param>
        /// <param name="merchantId">Identifies the merchant for which to return a list of locations</param>
        /// <returns>A list of locations for the specified merchant.</returns>
        /// <exception cref="LevelUpApi.LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        public IList<Location> ListLocations(string accessToken, int merchantId)
        {
            string locationsUri = HttpRest.CreateCombinedUrl(API_URL_BASE,
                                                             string.Format("merchants/{0}/locations", merchantId));

            IRestResponse response = LevelUpRestWrapper.Get(locationsUri, accessToken, Identifier.ToString());

            ThrowExceptionOnBadResponseCode(response);

            return JsonConvert.DeserializeObject<List<Location>>(response.Content);
        }

        /// <summary>
        /// Lists all the orders placed at a specified location
        /// </summary>
        /// <param name="accessToken">The LevelUp accesstoken obtained from the Authenticate() method</param>
        /// <param name="locationId">Identifies the location for which to return the list of orders</param>
        /// <returns>A list of detailed order data for the specified location</returns>
        /// <exception cref="LevelUpApi.LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        public IList<OrderDetailsResponse> ListOrders(string accessToken, int locationId)
        {
            string ordersByLocationUri = HttpRest.CreateCombinedUrl(API_URL_BASE,
                                                                    string.Format("locations/{0}/orders", locationId));

            IRestResponse response = LevelUpRestWrapper.Get(ordersByLocationUri, accessToken, Identifier.ToString());

            ThrowExceptionOnBadResponseCode(response);

            return JsonConvert.DeserializeObject<List<OrderDetailsResponse>>(response.Content);
        }

        /// <summary>
        /// Place an order and pay through LevelUp
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method. 
        /// This will be the .Token member on the returned object</param>
        /// <param name="orderData">An object containing the order data and spend amounts to process through LevelUp</param>
        /// <returns>A response object indicating whether the order was charged successfully and 
        /// the final amount paid including the customer specified tip amount</returns>
        /// <exception cref="LevelUpApi.LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        public OrderResponse PlaceOrder(string accessToken, Order orderData)
        {
            string placeOrderUri = HttpRest.CreateCombinedUrl(API_URL_BASE, "orders");

            // Create the body content of the order request
            string body = JsonConvert.SerializeObject(orderData);

            IRestResponse response = LevelUpRestWrapper.Post(placeOrderUri, body, accessToken, Identifier.ToString());

            ThrowExceptionOnBadResponseCode(response);

            return JsonConvert.DeserializeObject<OrderResponse>(response.Content);
        }

        /// <summary>
        /// Refund an order placed through LevelUp.
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method. 
        /// This will be the .Token member on the returned object</param>
        /// <param name="orderIdentifier">UUID identifier for the order to refund. 
        /// This information is returned when the order is placed</param>
        /// <param name="managerConfirmation">Some systems require manager confirmation. 
        /// This field should be omitted or set to null if your system does not require manager confirmation</param>
        /// <returns>A response object indicating whether the refund was successful</returns>
        /// <exception cref="LevelUpApi.LevelUpApiException">Thrown when the order to refund could not be found or 
        /// when the HTTP status returned was not 200 OK</exception>
        public RefundResponse RefundOrder(string accessToken, string orderIdentifier, string managerConfirmation = null)
        {
            string body = JsonConvert.SerializeObject(new Refund(managerConfirmation));

            string orderRefundUri = HttpRest.CreateCombinedUrl(API_URL_BASE,
                                                        string.Format("orders/{0}/refund", orderIdentifier));

            IRestResponse response = LevelUpRestWrapper.Post(orderRefundUri, body, accessToken, Identifier.ToString());

            if (response.StatusCode == HttpStatusCode.Unauthorized && string.IsNullOrEmpty(response.Content))
            {
                throw new LevelUpApiException(string.Format("The order \"{0}\" does not exist.", orderIdentifier),
                                              response.ErrorException);
            }

            ThrowExceptionOnBadResponseCode(response);

            return JsonConvert.DeserializeObject<RefundResponse>(response.Content);
        }

        #endregion ILevelUpApi

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Throws LevelUpApi.LevelUpApiException when the HTTP status code of the response is not 200 OK
        /// </summary>
        /// <param name="response">Response to check the response for.</param>
        private void ThrowExceptionOnBadResponseCode(IRestResponse response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new LevelUpApiException(string.Format("Error {0} ({1}). Endpoint \"{2}\" not found!",
                                                                (int) response.StatusCode, 
                                                                response.StatusCode,
                                                                response.ResponseUri));
                }

                List<ErrorResponse> errors = JsonConvert.DeserializeObject<List<ErrorResponse>>(response.Content);
                throw LevelUpApiException.Initialize(errors, response.StatusCode, response.ErrorException);
            }
        }

        #endregion Private Methods
    }
}
