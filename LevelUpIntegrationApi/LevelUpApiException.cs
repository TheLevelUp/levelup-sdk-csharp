using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace LevelUpApi
{
    public class LevelUpApiException : Exception
    {
        #region Static Members

        public static LevelUpApiException Initialize<T>(IList<T> messages, HttpStatusCode status, Exception innerException = null)
        {
            StringBuilder sb = new StringBuilder();

            if (null != messages)
            {
                foreach (var message in messages)
                {
                    sb.AppendLine(message.ToString());
                }
            }

            return new LevelUpApiException(sb.ToString(), status, innerException);
        }

        #endregion Static Members

        public LevelUpApiException(string message = "", Exception innerException = null)
            : base(message, innerException)
        {
        }

        public LevelUpApiException(string message, HttpStatusCode status, Exception innerException = null)
            : this(message, innerException)
        {
            this.StatusCode = status;
        }

        public HttpStatusCode StatusCode { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} Http Status: {1} [{2}]", base.ToString(), StatusCode, (int)StatusCode);
        }
    }
}
