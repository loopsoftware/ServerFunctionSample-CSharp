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
    using static System.Configuration.ConfigurationManager;

    public static class Configuration
    {
        /// <summary>Name of the Azure Active Directory instance.</summary>
        public static readonly string AzureADInstance = AppSettings["ida:AADInstance"];

        /// <summary>Name of the Azure AD tenant in which this application is registered.</summary>
        public static readonly string AzureTenantName = AppSettings["ida:Tenant"];

        /// <summary>Id of the client application registered with Azure AD.</summary>
        public static readonly string AzureClientId = AppSettings["ida:ClientId"];

        /// <summary>The application key is a credential used by the application to authenticate to Azure AD.</summary>
        public static readonly string AzureAppKey = AppSettings["ida:AppKey"];

        /// <summary>The URI of the resource to access.</summary>
        public static readonly string AzureResourceUri = AppSettings["ida:ResourceUri"];

        /// <summary>The redirect URI of the Azure client application.</summary>
        public static readonly string AzureRedirectUri = AppSettings["ida:RedirectUri"];

        /// <summary>URL of the authority issuing the access token.</summary>
        public static readonly string AzureAuthorityUrl = string.Format("{0}/{1}", AzureADInstance, AzureTenantName);

        /// <summary>URL to cal lto get the version of loop to use.</summary>
        public static readonly string LoopVersionUrl = AppSettings["loop:VersionUrl"];

        /// <summary>The ID of the session to use.</summary>
        public static readonly string LoopSessionId = AppSettings["loop:SessionId"];

        /// <summary>URL to invoke the Loop server function.</summary>
        public static readonly string LoopServerFunctionUrl = AppSettings["loop:ServerFunction"];

        /// <summary>Username to login to Azure: UPN format (i.e., user email).</summary>
        public static readonly string LoopUsername = AppSettings["loop:Username"];

        /// <summary>Password for the user to login to Azure.</summary>
        public static readonly string LoopPassword = AppSettings["loop:Password"];
    }
}
