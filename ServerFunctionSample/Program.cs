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
using System.Diagnostics;

namespace Fr.LoopSoftware.Sample.ServerFunction
{
    using System;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using static AzureConfiguration;

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
                var context = new AuthenticationContext(Authority);
                if (context != null)
                {
                    var credentials = new ClientCredential(ClientId, AppKey);
                    if (credentials != null)
                    {
                        var result = await context.AcquireTokenAsync(ResourceUri, credentials);
                        if (result != null)
                        {
                            string token = result.AccessToken;
                            string tokenType = result.AccessTokenType;
                            string expiration = result.ExpiresOn.ToString();

                            Console.Out.WriteLine(">>> Authenticated successfully...");
                            Console.Out.WriteLine(">>> Token type: " + tokenType);
                            Console.Out.WriteLine(">>> Token: " + token);
                            Console.Out.WriteLine(">>> Expiration: " + expiration);

                            // TODO: send token to Loop server...

                            // TODO: use received session to call server function REST API...

                            // TODO: show result of server function call... ??

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
    }
}
