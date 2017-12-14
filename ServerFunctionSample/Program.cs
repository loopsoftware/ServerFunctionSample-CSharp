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
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Newtonsoft.Json.Linq;
    using static Configuration;

    class MainClass
    {
        public static void Main(string[] args)
        {
            Run();

            do
            {
                Console.WriteLine("Press Q to exit...");
            }
            while (Console.ReadKey().Key != ConsoleKey.Q);
        }

        private static async void Run()
        {
            int code = -1;
            try
            {
                var context = new AuthenticationContext(AzureAuthorityUrl);

                var result = await context.AcquireTokenAsync(AzureResourceUri, AzureClientId, new Uri(AzureRedirectUri), new PlatformParameters(PromptBehavior.Auto));

                if (result != null)
                {
                    string token = result.AccessToken;
                    string tokenType = result.AccessTokenType;
                    string expiration = result.ExpiresOn.ToString();

                    Console.Out.WriteLine(">>> Authenticated successfully...");
                    Console.Out.WriteLine(">>> Token type: " + tokenType);
                    Console.Out.WriteLine(">>> Token: " + token);
                    Console.Out.WriteLine(">>> Expiration: " + expiration);

                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, token);

                        // request an authenticated session from the Loop server...
                        using (var loopAuthorisationResponse = await httpClient.GetAsync(new Uri(LoopAuthorisationUrl)))
                        {
                            loopAuthorisationResponse.EnsureSuccessStatusCode();

                            // use received session to call server function REST API...
                            string sessionJson = await loopAuthorisationResponse.Content.ReadAsStringAsync();

                            var jsonObject = JObject.Parse(sessionJson);

                            string sessionId = jsonObject.GetValue("sessionId").Value<string>();
                            string user = jsonObject.GetValue("user").Value<string>();

                            Console.Out.WriteLine(">>> Retrieved authorised session from Loop...");
                            Console.Out.WriteLine(">>> session: " + sessionId);
                            Console.Out.WriteLine(">>> user: " + user);

                            // TODO: show result of server function call... ??
                            using (var loopServerFunctionResponse = await httpClient.GetAsync(new Uri(LoopServerFunctionUrl)))
                            {
                                loopServerFunctionResponse.EnsureSuccessStatusCode();

                                string serverFunctionResult = await loopServerFunctionResponse.Content.ReadAsStringAsync();

                                Console.Out.WriteLine(">>> Server function result: " + serverFunctionResult);

                                code = 0;
                            }
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
    }
}
