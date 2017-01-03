#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="PaymentToken.cs" company="SCVNGR, Inc. d/b/a LevelUp">
//   Copyright(c) 2016 SCVNGR, Inc. d/b/a LevelUp. All rights reserved.
// </copyright>
// <license publisher="Apache Software Foundation" date="January 2004" version="2.0">
//   Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except
//   in compliance with the License. You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software distributed under the License
//   is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//   or implied. See the License for the specific language governing permissions and limitations under
//   the License.
// </license>
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
#endregion

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

                return qrCodeToValidate.StartsWith(Constants.PaymentTokenSentinel,
                                                   StringComparison.OrdinalIgnoreCase) &&
                       qrCodeToValidate.EndsWith(Constants.PaymentTokenSentinel,
                                                 StringComparison.OrdinalIgnoreCase) &&
                       qrCodeToValidate.Length == 32 &&
                       qrCodeToValidate.IndexOfAny(new[] {' ', '\r', '\n'}) == -1;
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

            public static string StripTip(string qrCode)
            {
                if (string.IsNullOrEmpty(qrCode))
                {
                    return string.Empty;
                }

                const int tipStartIndex = 27;
                const int tipEncodingLength = 2;
                const string zeroTipEncoded = "00";

                qrCode = qrCode.Trim();

                return qrCode.Length < (tipStartIndex + tipEncodingLength)
                           ? qrCode
                           : qrCode.Substring(0, tipStartIndex) + zeroTipEncoded +
                             qrCode.Substring(tipStartIndex + tipEncodingLength);
            }
        }
    }
}
