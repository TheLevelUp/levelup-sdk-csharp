
using System.Text;

namespace LevelUpApi
{
    public class AgentIdentifier
    {
        private const int LENGTH_LIMIT = 20; //20 Character length limit for each field
        private string _companyName = string.Empty;
        private string _productName = string.Empty;
        private string _productVersion = string.Empty;
        private string _osName = string.Empty;

        /// <summary>
        /// Class that holds identifying information for the individual or group who is developing against the 
        /// LevelUp API. This information may be used to more easily identify the source of requests made to LevelUp.
        /// This information will be sent as part of each request to LevelUp in the User-Agent header field.
        /// </summary>
        /// <param name="companyName">Name of the individual, group, or company name developing this software</param>
        /// <param name="productName">Name of the product or tool into which this software will be built</param>
        /// <param name="productVersion">Version of the product or tool of which this software is a part</param>
        /// <param name="osName">Name of the operating system on which this software runs</param>
        public AgentIdentifier(string companyName, 
            string productName,
            string productVersion,
            string osName)
        {
            this.CompanyName = companyName;
            this.ProductName = productName;
            this.ProductVersion = productVersion;
            this.OsName = osName;
        }

        /// <summary>
        /// Name of the individual, group, or company developing this software
        /// </summary>
        public string CompanyName
        {
            get { return _companyName; }
            set { _companyName = TrimStringToLength(value, LENGTH_LIMIT).Replace("\n", string.Empty); }
        }

        /// <summary>
        /// Name of the product or tool of which this software is a part
        /// </summary>
        public string ProductName
        {
            get { return _productName; }
            set { _productName = TrimStringToLength(value, LENGTH_LIMIT).Replace("\n", string.Empty); }
        }

        /// <summary>
        /// Version of the product or tool of which this software is a part
        /// </summary>
        public string ProductVersion
        {
            get { return _productVersion; }
            set { _productVersion = TrimStringToLength(value, LENGTH_LIMIT).Replace("\n", string.Empty); }
        }

        /// <summary>
        /// Name of the operating system on which this software runs
        /// </summary>
        public string OsName
        {
            get { return _osName; }
            set { _osName = TrimStringToLength(value, LENGTH_LIMIT).Replace("\n", string.Empty); }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            //Desired format: "CompanyName ProductName/ProductVersion OsName"
            if (!string.IsNullOrEmpty(CompanyName))
            {
                sb.AppendFormat("{0}-", CompanyName);
            }

            if (!string.IsNullOrEmpty(ProductName))
            {
                sb.Append(ProductName);

                if (string.IsNullOrEmpty(ProductVersion))
                {
                    sb.Append(ProductVersion);
                }
                else
                {
                    sb.AppendFormat("/{0}", ProductVersion);
                }
            }

            if (!string.IsNullOrEmpty(OsName))
            {
                sb.AppendFormat(" {0}", OsName);
            }

            return sb.ToString();
        }

        private string TrimStringToLength(string stringToTrim, int lengthLimit)
        {
            return (stringToTrim.Length > LENGTH_LIMIT) ? stringToTrim.Substring(0, lengthLimit) : stringToTrim;
        }
    }
}
