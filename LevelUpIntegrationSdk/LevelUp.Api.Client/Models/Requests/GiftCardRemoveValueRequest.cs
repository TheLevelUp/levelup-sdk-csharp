using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models.Requests
{
    /// <summary>
    /// Class representing a LevelUp gift card remove value request
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class GiftCardRemoveValueRequest
    {
        private GiftCardRemoveValueRequest()
        {
            //Private constructor for deserialization
        }

        /// <summary>
        /// Creates a destroy value request for a LevelUp gift card
        /// </summary>
        /// <param name="giftCardQrData">The qr code of the target card or account</param>
        /// <param name="amountInCents">The amount of value to destroy in US Cents</param>
        public GiftCardRemoveValueRequest(string giftCardQrData, int amountInCents)
            : this(new GiftCardValue(giftCardQrData, amountInCents))
        {
        }

        /// <summary>
        /// Creates a destroy value request for a LevelUp gift card. 
        /// </summary>
        /// <param name="giftCardValue">An object containing the qr data identifying the target card or account
        /// as well as the amount to destroy in cents</param>
        public GiftCardRemoveValueRequest(GiftCardValue giftCardValue)
        {
            GiftCardValue = giftCardValue;
        }

        /// <summary>
        /// The amount of value to be removed from the gift card in US Cents. 
        /// This must be a positive amount and should not exceed the amount available on the gift card.
        /// </summary>
        public int AmountInCents
        {
            get { return GiftCardValue.AmountInCents; }
            set { GiftCardValue.AmountInCents = value; }
        }

        /// <summary>
        /// QR Data identifying the gift card or user account
        /// </summary>
        public string GiftCardQrData
        {
            get { return GiftCardValue.PaymentData; }
            set { GiftCardValue.PaymentData = value; }
        }

        [JsonProperty(PropertyName = "gift_card_value_removal")]
        private GiftCardValue GiftCardValue { get; set; }
    }
}
