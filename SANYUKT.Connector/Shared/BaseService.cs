using SANYUKT.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace SANYUKT.Connector.Shared
{
    public class BaseService
    {
        protected APIConnector apiHelper = new APIConnector();

        public BaseService()
        {
            apiHelper.BaseUrl = SANYUKTApplicationConfiguration.Instance.FIAAPIUrl;
        }

        public string URLEncode(string Param)
        {
            return System.Net.WebUtility.UrlEncode(Param);
        }

        public string URLEncode(object Param)
        {
            if (Param == null)
                return "";

            return System.Net.WebUtility.UrlEncode(Param.ToString());
        }
    }
}
