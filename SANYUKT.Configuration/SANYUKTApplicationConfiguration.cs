using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;


namespace SANYUKT.Configuration
{
    public class SANYUKTApplicationConfiguration
    {
        private static readonly SANYUKTApplicationConfiguration instance = new SANYUKTApplicationConfiguration();
        private static IHostingEnvironment _HostingEnvironment = null;
        private static IConfigurationRoot configuration = null;

        private SANYUKTApplicationConfiguration()
        {
        }

        static SANYUKTApplicationConfiguration()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            
            if (File.Exists(Directory.GetCurrentDirectory() + "\\" + "appsettings.development.json"))
                builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true);
            else
                builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);


            configuration = builder.Build();
        }


        public static SANYUKTApplicationConfiguration Instance
        {
            get
            {
                return instance;
            }
        }

        public string FIADB
        {
            get
            {
                return configuration.GetConnectionString("FIADB");
            }
        }
        public string FileDownloadPath
        {
            get
            {
                return configuration["FileDownloadPath"];
            }
        }
        public string AuditingDB
        {
            get
            {
                return configuration.GetConnectionString("AuditingDB");
            }
        }
        public string LoggingDB
        {
            get
            {
                return configuration.GetConnectionString("LoggingDB");
            }
        }
        public string FIAAPIUrl
        {
            get
            {
                return configuration["FIAAPIUrl"];
            }
        }

        public string FIABillBayDB
        {
            get
            {
                return configuration.GetConnectionString("FIABillBayDB");
            }
        }

        public string FileUploadPath
        {
            get
            {
                return configuration["FileUploadPath"];
            }
        }

      
        public string APIToken
        {
            get
            {
                return configuration["APIToken"];
            }
        }

        public string RblPayoutBaseUrl
        {
            get
            {
                return configuration["RblBankPayout:BaseUrl"];
            }
        }
        public string RblPayoutusername
        {
            get
            {
                return configuration["RblBankPayout:username"];
            }
        }
        public string RblPayoutPass
        {
            get
            {
                return configuration["RblBankPayout:pass"];
            }
        }
        public string RblPayoutclientId
        {
            get
            {
                return configuration["RblBankPayout:client_id"];
            }
        }
        public string RblPayoutclientSecrat
        {
            get
            {
                return configuration["RblBankPayout:client_secret"];
            }
        }
        public string RblPayoutCORPID
        {
            get
            {
                return configuration["RblBankPayout:CORPID"];
            }
        }
       
        public string certisslpass
        {
            get
            {
                return configuration["CertSslPass"];
            }
        }
        public string certisslName
        {
            get
            {
                return configuration["CertSslName"];
            }
        }
       
        public string RblAccountNo
        {
            get
            {
                return configuration["RblBankPayout:AccountNo"];
            }
        }
        public string RblAccountName
        {
            get
            {
                return configuration["RblBankPayout:AccountName"];
            }
        }
        public string RblPayoutIfsccode
        {
            get
            {
                return configuration["RblBankPayout:DebitIfsc"];
            }
        }
        public string RblPayoutMobile
        {
            get
            {
                return configuration["RblBankPayout:DebitMobile"];
            }
        }
        public string digitapApiURL
        {
            get
            {
                return configuration["digitapApiURL"];
            }
        }
        public string digitapClientID
        {
            get
            {
                return configuration["digitapClientID"];
            }
        }
        public string digitapSecratekey
        {
            get
            {
                return configuration["digitapSecratekey"];
            }
        }
        public string PaysprintjwtToken
        {
            get
            {
                return configuration["Paysprint:jwtToken"];
            }
        }
        public string PaysprintAuthKey
        {
            get
            {
                return configuration["Paysprint:AuthKey"];
            }
        }
        public string PaysprintPartnerId
        {
            get
            {
                return configuration["Paysprint:PartnerId"];
            }
        }
        public string PaysprintBaseUrl
        {
            get
            {
                return configuration["Paysprint:BaseUrl"];
            }
        }
        public string PaysprintAESENCRYPTIONIV
        {
            get
            {
                return configuration["Paysprint:AESENCRYPTIONIV"];
            }
        }
        public string PaysprintAESENCRYPTIONKEY
        {
            get
            {
                return configuration["Paysprint:AESENCRYPTIONKEY"];
            }
        }
        public string CMSCertificate
        {
            get
            {
                return configuration["CMSCertificate"];
            }
        }
        public string CMSMerchantId
        {
            get
            {
                return configuration["CMSMerchantId"];
            }
        }
        public string CMSMerchantLoginId
        {
            get
            {
                return configuration["CMSMerchantLoginId"];
            }
        }
        public string CMSMerchantPass
        {
            get
            {
                return configuration["CMSMerchantPass"];
            }
        }
        public string CMSpassword
        {
            get
            {
                return configuration["CMSpassword"];
            }
        }
        public string CMSAgreegator
        {
            get
            {
                return configuration["CMSAgreegator"];
            }
        }
        public string CMSAPIURL
        {
            get
            {
                return configuration["CMSAPIURL"];
            }
        }
        public string CMSMerchantSecurityKey
        {
            get
            {
                return configuration["CMSMerchantSecurityKey"];
            }
        }

        public string CMSsuperMerchantSkey
        {
            get
            {
                return configuration["CMSsuperMerchantSkey"];
            }
        }
        public string CMSSingleSignOnURL
        {
            get
            {
                return configuration["CMSSingleSignOnURL"];
            }
        }
        public string CMSAPIMerURL
        {
            get
            {
                return configuration["CMSAPIMerURL"];
            }
        }

        public string CMSAPIMerCorporateServiceURL
        {
            get
            {
                return configuration["CMSAPIMerCorporateServiceURL"];
            }
        }

        public string CMSAPIKYCURL
        {
            get
            {
                return configuration["CMSAPIKYCURL"];
            }
        }

        private string GetParameterValue(string ParamName)
        {
            if (configuration[ParamName] != null)
                return configuration[ParamName];
            else
                return "";
        }
      
    }
}
