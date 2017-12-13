//  Copyright 2017  Yupana Systems
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

namespace Fr.LoopSoftware.Sample.ServerFunction
{
    public static class AzureConfiguration
    {
        /// <summary>Name of the Azure Active Directory instance.</summary>
        public static readonly string AzureADInstance = System.Configuration.ConfigurationManager.AppSettings["ida:AADInstance"];

        /// <summary>Name of the Azure AD tenant in which this application is registered.</summary>
        public static readonly string TenantName = System.Configuration.ConfigurationManager.AppSettings["ida:Tenant"];

        /// <summary>The application's redirect URI.</summary>
        public static readonly string RedirectUri = System.Configuration.ConfigurationManager.AppSettings["ida:RedirectUri"];

        /// <summary>Id of the client application registered with Azure AD.</summary>
        public static readonly string ClientId = System.Configuration.ConfigurationManager.AppSettings["ida:ClientId"];

        /// <summary>The application key is a credential used by the application to authenticate to Azure AD.</summary>
        public static readonly string AppKey = System.Configuration.ConfigurationManager.AppSettings["ida:AppKey"];

        /// <summary>The URI of the resource to access.</summary>
        public static readonly string ResourceUri = System.Configuration.ConfigurationManager.AppSettings["ida:ResourceUri"];

        /// <summary>URL of the authority issuing the access token.</summary>
        public static readonly string Authority = string.Format("{0}/{1}", AzureADInstance, TenantName);
    }
}
