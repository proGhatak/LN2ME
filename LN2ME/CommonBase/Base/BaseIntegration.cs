using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Security.Principal;

namespace CommonBase.Base
{
    public abstract class BaseIntegration
    {
        #region Constructors

        public BaseIntegration() { }

        #endregion

        #region Private Properties / Variables

        //private IOrganizationService organizationService = null;

        #endregion

        #region Protected Properties / Variables

        protected WSHttpSecurity wsHttpSecurity = null;
        protected WebHttpSecurity webHttpSecurity = null;
        protected BasicHttpSecurity basicHttpSecurity = null;
        protected ClientCredentials clientCredentials = null;

        /// <summary>
        /// This method is used for client credentails and sets the value 
        /// </summary>
        protected virtual ClientCredentials ClientCredentials
        {
            get
            {
                if (clientCredentials == null) clientCredentials = new ClientCredentials();
                return clientCredentials;
            }
            set { clientCredentials = value; }
        }

        /// <summary>
        /// This method is used to check Organization Service in portal crm congiguration
        /// </summary>
        //protected IOrganizationService OrganizationService
        //{
        //    get
        //    {
        //        if (organizationService == null) organizationService = Common.Helper.CrmConfigurationManager.CreateOrganizationService();
        //        return organizationService;
        //    }
        //}

        #endregion

        #region Protected Subs / Functions

        protected virtual CustomBinding GetCustomBinding(string configName)
        {
            return GetCustomBinding(configName, AuthenticationSchemes.Anonymous);
        }

        /// <summary>
        /// This method is used to get the custom binding using list of binding elements
        /// </summary>
        /// <param name="configName"></param>
        /// <param name="authenticationScheme"></param>
        /// <returns>GetCustomBinding</returns>
        protected virtual CustomBinding GetCustomBinding(string configName, AuthenticationSchemes authenticationScheme)
        {
            List<BindingElement> bindingElements = new List<BindingElement>();

            MtomMessageEncodingBindingElement mtomMessageEncoding = new MtomMessageEncodingBindingElement()
            {
                MaxReadPoolSize = 64,
                MaxWritePoolSize = 16,
                MaxBufferSize = 65536,
                MessageVersion = System.ServiceModel.Channels.MessageVersion.Soap11WSAddressing10,
                WriteEncoding = System.Text.Encoding.UTF8,
            };
            bindingElements.Add(mtomMessageEncoding);

            HttpTransportBindingElement httpTransport = new HttpTransportBindingElement()
            {
                ManualAddressing = false,
                MaxBufferPoolSize = 524288,
                MaxReceivedMessageSize = 65536,
                AllowCookies = false,
                AuthenticationScheme = authenticationScheme,
                BypassProxyOnLocal = false,
                DecompressionEnabled = true,
                HostNameComparisonMode = HostNameComparisonMode.StrongWildcard,
                KeepAliveEnabled = true,
                MaxBufferSize = 65536,
                ProxyAuthenticationScheme = authenticationScheme,
                Realm = string.Empty,
                TransferMode = TransferMode.Buffered,
                UnsafeConnectionNtlmAuthentication = false,
                UseDefaultWebProxy = true,
            };
            bindingElements.Add(httpTransport);

            CustomBinding customBinding = new CustomBinding(bindingElements)
            {
                Name = configName,
                OpenTimeout = new TimeSpan(0, 2, 0),
                CloseTimeout = new TimeSpan(0, 2, 0),
                SendTimeout = new TimeSpan(0, 5, 0),
                ReceiveTimeout = new TimeSpan(0, 5, 0),
            };

            return customBinding;
        }

        protected virtual WSHttpBinding GetWSHttpBinding(string configName, Boolean secured)
        {
            if (wsHttpSecurity == null)
            {
                wsHttpSecurity = new WSHttpSecurity()
                {
                    Mode = (secured) ? SecurityMode.TransportWithMessageCredential : SecurityMode.None,
                    Transport = new HttpTransportSecurity()
                    {
                        ClientCredentialType = (secured) ? HttpClientCredentialType.None : HttpClientCredentialType.Basic,
                        ProxyCredentialType = (secured) ? HttpProxyCredentialType.None : HttpProxyCredentialType.Basic,
                        Realm = string.Empty,
                    },
                    Message = new NonDualMessageSecurityOverHttp()
                    {
                        ClientCredentialType = (secured) ? MessageCredentialType.UserName : MessageCredentialType.Windows
                    },
                };
            }

            WSHttpBinding wsHttpBinding = new WSHttpBinding()
            {
                Name = configName,
                MaxBufferPoolSize = 2147483647,
                MaxReceivedMessageSize = 2147483647,
                OpenTimeout = new TimeSpan(0, 2, 0),
                CloseTimeout = new TimeSpan(0, 2, 0),
                SendTimeout = new TimeSpan(0, 5, 0),
                ReceiveTimeout = new TimeSpan(0, 5, 0),
                ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas()
                {
                    MaxDepth = 32,
                    MaxStringContentLength = 2147483647,
                    MaxArrayLength = 16348,
                    MaxBytesPerRead = 4096,
                    MaxNameTableCharCount = 16384,
                },
                Security = wsHttpSecurity,
            };

            return wsHttpBinding;
        }

        protected virtual WebHttpBinding GetWebHttpBinding(string configName, Boolean secured)
        {
            if (webHttpSecurity == null)
            {
                webHttpSecurity = new WebHttpSecurity()
                {
                    Mode = (secured) ? WebHttpSecurityMode.Transport : WebHttpSecurityMode.None,
                    Transport = new HttpTransportSecurity()
                    {
                        ClientCredentialType = (secured) ? HttpClientCredentialType.None : HttpClientCredentialType.Basic,
                        ProxyCredentialType = (secured) ? HttpProxyCredentialType.None : HttpProxyCredentialType.Basic,
                        Realm = string.Empty,
                    },
                };
            }

            WebHttpBinding webHttpBinding = new WebHttpBinding()
            {
                Name = configName,
                MaxBufferPoolSize = 2147483647,
                MaxReceivedMessageSize = 2147483647,
                OpenTimeout = new TimeSpan(0, 2, 0),
                CloseTimeout = new TimeSpan(0, 2, 0),
                SendTimeout = new TimeSpan(0, 5, 0),
                ReceiveTimeout = new TimeSpan(0, 5, 0),
                ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas()
                {
                    MaxDepth = 32,
                    MaxStringContentLength = 2147483647,
                    MaxArrayLength = 16348,
                    MaxBytesPerRead = 4096,
                    MaxNameTableCharCount = 16384,
                },
                Security = webHttpSecurity,
            };

            return webHttpBinding;
        }

        protected virtual BasicHttpBinding GetBasicHttpBinding(string configName, Boolean secured)
        {
            if (basicHttpSecurity == null)
            {
                basicHttpSecurity = new BasicHttpSecurity()
                {
                    Mode = (secured) ? BasicHttpSecurityMode.Transport : BasicHttpSecurityMode.None,
                    Transport = new HttpTransportSecurity()
                    {
                        ClientCredentialType = (secured) ? HttpClientCredentialType.None : HttpClientCredentialType.Basic,
                        ProxyCredentialType = (secured) ? HttpProxyCredentialType.None : HttpProxyCredentialType.Basic,
                        Realm = string.Empty,
                    },
                };
            }

            BasicHttpBinding basicHttpBinding = new BasicHttpBinding()
            {
                Name = configName,
                MaxBufferPoolSize = 2147483647,
                MaxReceivedMessageSize = 2147483647,
                OpenTimeout = new TimeSpan(0, 2, 0),
                CloseTimeout = new TimeSpan(0, 2, 0),
                SendTimeout = new TimeSpan(0, 5, 0),
                ReceiveTimeout = new TimeSpan(0, 5, 0),
                ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas()
                {
                    MaxDepth = 32,
                    MaxStringContentLength = 2147483647,
                    MaxArrayLength = 16348,
                    MaxBytesPerRead = 4096,
                    MaxNameTableCharCount = 16384,
                },
                Security = basicHttpSecurity,
            };
            //TRUST ALL CERTIFICATES
            ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);

            return basicHttpBinding;
        }

        protected TokenImpersonationLevel GetTokenImpersonationLevel()
        {
            return GetTokenImpersonationLevel(null);
        }

        protected TokenImpersonationLevel GetTokenImpersonationLevel(ClientCredentials clientCredentials)
        {
            System.Security.Principal.TokenImpersonationLevel tokenImpersonationLevel = TokenImpersonationLevel.Impersonation;

            if (clientCredentials != null) tokenImpersonationLevel = clientCredentials.Windows.AllowedImpersonationLevel;

            //if (!string.IsNullOrEmpty(Constants.AppSettings.RetrieveAppSettingValue("tokenimpersonationlevel")))
            //{
            //    try
            //    {
            //        tokenImpersonationLevel = (TokenImpersonationLevel)Enum.Parse(typeof(TokenImpersonationLevel), Constants.AppSettings.RetrieveAppSettingValue("tokenimpersonationlevel"), true);
            //    }
            //    //catch (Exception ex) { Common.Helper.LogManager.Instance.LogException(ex, "BaseIntegration.GetTokenImpersonationLevel"); }
            //}                

            return tokenImpersonationLevel;
        }

        /// <summary>
        /// This method is used for Tibco Integration enabling
        /// </summary>
        //protected Boolean TibcoIntegrationEnabled(string name = default(string))
        //{
        //    Boolean tibcoIntegrationEnabled = false
        //            , useDefaultTibcoIntegrationEnabled = true;

        //    //String empty validation
        //    if (!string.IsNullOrEmpty(name))
        //    {
        //    //    string _tibcoIntegrationEnabled = Common.Helper.HelperFunctions.GetSettingValue(string.Format("tibcointegration.{0}.enabled", name));

        //    //    //String empty validation
        //    //    if (!string.IsNullOrEmpty(_tibcoIntegrationEnabled))
        //    //    {
        //    //        tibcoIntegrationEnabled = _tibcoIntegrationEnabled.ToLowerInvariant().Equals("true");
        //    //        useDefaultTibcoIntegrationEnabled = false;
        //    //    }                
        //    //}            
        //    //if (useDefaultTibcoIntegrationEnabled) tibcoIntegrationEnabled = Common.Helper.HelperFunctions.GetSettingValue("tibcointegration.enabled", "true").ToLowerInvariant().Equals("true");

        //    return tibcoIntegrationEnabled;
        //}

        #endregion
    }
}