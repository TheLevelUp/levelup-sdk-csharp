using System;

namespace LevelUp.Api.Utilities
{
    public class PaymentToken
    {
        public class QrCode
        {
            /// <summary>
            /// Method to make a very ROUGH determination concerning whether a given QR code is valid as a LevelUp 
            /// payment token
            /// This method is to be used as a client side basic sanity check. It is NOT guaranteed to be accurate
            /// </summary>
            /// <param name="qrCodeToValidate">The Qr Code to validate</param>
            /// <returns>True if the qr code has the same APPROXIMATE format as a LevelUp QrCode payment token</returns>
            public static bool IsProbablyValid(string qrCodeToValidate)
            {
                if (string.IsNullOrEmpty(qrCodeToValidate))
                {
                    return false;
                }

                qrCodeToValidate = qrCodeToValidate.Trim();

                return qrCodeToValidate.StartsWith(Constants.PAYMENT_TOKEN_SENTINEL,
                                                   StringComparison.OrdinalIgnoreCase) &&
                       qrCodeToValidate.EndsWith(Constants.PAYMENT_TOKEN_SENTINEL,
                                                 StringComparison.OrdinalIgnoreCase) &&
                       qrCodeToValidate.Length == 32 &&
                       qrCodeToValidate.IndexOfAny(new []{' ', '\r', '\n'}) == -1;
            }

            public static string Redact(string unsanitizedQrCode)
            {
                if (string.IsNullOrEmpty(unsanitizedQrCode))
                {
                    return string.Empty;
                }

                //QR Code Spec Document: http://bit.ly/1DZJWPk
                const int paymentTokenStartIndex = 11;
                const int paymentTokenLength = 13;

                unsanitizedQrCode = unsanitizedQrCode.Trim();

                return unsanitizedQrCode.Length <= (paymentTokenStartIndex + paymentTokenLength)
                           ? unsanitizedQrCode
                           : unsanitizedQrCode.Replace(unsanitizedQrCode.Substring(paymentTokenStartIndex,
                                                                                   paymentTokenLength),
                                                       "[** Redacted **]");
            }
        }
    }
}
