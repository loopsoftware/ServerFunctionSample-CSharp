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
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Newtonsoft.Json.Linq;
    using static Configuration;

    class MainClass
    {
        public static void Main(string[] args)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Run();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            do
            {
                Console.WriteLine("Press Q to exit...");
            }
            while (Console.ReadKey().Key != ConsoleKey.Q);
        }

        private static async Task Run()
        {
            int code = -1;
            try
            {
                var context = new AuthenticationContext(AzureAuthorityUrl);

                // generate an credential to user for authentication with azure when asking for an access token...
                var credential = new UserPasswordCredential(LoopUsername, LoopPassword);

                // request an access token from azure...
                var result = await context.AcquireTokenAsync(AzureResourceUri, AzureClientId, credential);
                if (result != null)
                {
                    string token = result.AccessToken;
                    string tokenType = result.AccessTokenType;
                    string expiration = result.ExpiresOn.ToString();

                    Console.Out.WriteLine(">>> Authenticated successfully...");
                    Console.Out.WriteLine(">>> Token type: " + tokenType);
                    Console.Out.WriteLine(">>> Token: " + token);
                    Console.Out.WriteLine(">>> Expiration: " + expiration);

                    // create an http client...
                    using (var httpClient = new HttpClient())
                    {
                        // set the authorisation header with the access token received above...
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, token);

                        string version = await GetVersion(httpClient);
                        string sessionId = GetSessionId();

                        httpClient.DefaultRequestHeaders.Add("Cookie", "sessionId=" + sessionId);

                        string serverFunctionUrlWithVersion = string.Format(LoopServerFunctionUrl, version, sessionId);
                        using (var loopServerFunctionResponse = await httpClient.GetAsync(new Uri(LoopServerFunctionUrl)))
                        {
                            loopServerFunctionResponse.EnsureSuccessStatusCode();

                            var resultJson = JObject.Parse(await loopServerFunctionResponse.Content.ReadAsStringAsync());

                            // TODO: do something with the result...

                            code = 0;
                        }
                    }
                }
            }
            catch (AdalException exception)
            {
                Console.Error.WriteLine("Azure Active Directory Error: " + exception.Message);
                Console.Error.WriteLine(exception.StackTrace);

                code = -2;
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine("Error: " + exception.Message);
                Console.Error.WriteLine(exception.StackTrace);

                code = -3;
            }

            Environment.Exit(code);
        }

        private static async Task<string> GetVersion(HttpClient httpClient)
        {
            using (var loopVersionResponse = await httpClient.GetAsync(new Uri(LoopVersionUrl)))
            {
                loopVersionResponse.EnsureSuccessStatusCode();

                var versionJson = JObject.Parse(await loopVersionResponse.Content.ReadAsStringAsync());
                // get the version from the json...
                string loopVersion = Convert.ToString(versionJson["version"]);

                return loopVersion;
            }
        }

        private static string GetSessionId()
        {
            return "";
        }
    }
}
