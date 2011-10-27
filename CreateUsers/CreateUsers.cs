using System;
using System.Configuration;
using System.IO;

namespace CreateUsers
{
    internal class CreateUsers
    {
        private static readonly string ApiVersion = ConfigurationManager.AppSettings.Get(0);
        private static readonly string ApiURL = ConfigurationManager.AppSettings.Get(1);
        private static readonly string AccountURL = ConfigurationManager.AppSettings.Get(2);
        private static readonly string OauthRequestTokenURL = ConfigurationManager.AppSettings.Get(3);
        private static readonly string OauthAccesstokenURL = ConfigurationManager.AppSettings.Get(4);
        private static readonly string UserAuthenticationURL = ConfigurationManager.AppSettings.Get(5);
        private static readonly string SketchpadURL = ConfigurationManager.AppSettings.Get(6);
        private static readonly string ConsumerKey = ConfigurationManager.AppSettings.Get(7);
        private static readonly string ConsumerSecret = ConfigurationManager.AppSettings.Get(8);
        private static readonly string Password = ConfigurationManager.AppSettings.Get(9);
        private static readonly int Users = Convert.ToInt32(ConfigurationManager.AppSettings.Get(10));
        private static readonly string UserEmailPrefix = ConfigurationManager.AppSettings.Get(11);
        private static int _usersCreated;
        private static int _usersFailed;
        

        public static void Main(string[] args)
        {
            
            var oauthTokens = new string[Users];
            var oauthTokenSecrets = new string[Users];

            int oauthTokensIndex = 0;
            int oauthTokenSecretsIndex = 0;

            ResetUsersTotals();

            for (int i = 0; i < Users; i++)
            {
                
                var sketchpad = new Sketchpad();
                sketchpad.Setup();

                if (sketchpad.Driver != null)
                {
                    sketchpad.Driver.Navigate().GoToUrl(SketchpadURL);


                    if (sketchpad.PopulateAndSignSketchpad(ApiURL + "/" + ApiVersion +"/"+ OauthRequestTokenURL,
                                                           ConsumerKey,
                                                           ConsumerSecret))
                    {

                        string tempOauthToken = sketchpad.ExtractOauthToken();
                        string tempOauthTokenSecret = sketchpad.ExtractOauthTokenSecret();

                        if (sketchpad.UserAuthentication(AccountURL + "/" + ConsumerKey +"/"+ UserAuthenticationURL +
                                                         "?oauth_token=" +
                                                         tempOauthToken +
                                                         "&oauth_callback=http%3a%2f%2fapi-consumer.com%2fhandback%3fconsumerparam%3dvalue"))
                        {
                            sketchpad.Driver.Navigate().GoToUrl(SketchpadURL);
                            if (sketchpad.PopulateAndSignSketchpad(ApiURL + "/" + ApiVersion + "/" + OauthAccesstokenURL,
                                                                   ConsumerKey,
                                                                   ConsumerSecret, tempOauthToken,
                                                                   tempOauthTokenSecret))
                            {

                                string oauthToken = sketchpad.ExtractOauthToken();
                                string oauthTokenSecret = sketchpad.ExtractOauthTokenSecret();

                                if (oauthToken.EndsWith("==") && oauthTokenSecret.EndsWith("=="))
                                {
                                    _usersCreated = _usersCreated + 1;


                                    oauthTokens[oauthTokensIndex] = oauthToken;
                                    oauthTokensIndex = oauthTokensIndex + 1;
                                    Console.WriteLine(oauthToken);
                                    oauthTokenSecrets[oauthTokenSecretsIndex] = oauthTokenSecret;
                                    oauthTokenSecretsIndex = oauthTokenSecretsIndex + 1;
                                    Console.WriteLine(oauthTokenSecret);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Firefox is not installed or has been closed by the user");
                }
                sketchpad.Teardown();
            }
            Console.WriteLine("Created " + _usersCreated + " users");
            Console.WriteLine("Failed to create " + _usersFailed + " users");
            WirteToCsv(oauthTokens, oauthTokenSecrets);
        }

        public static void UpdateUsersFailed()
        {
            _usersFailed = _usersFailed + 1;
        }

        private static void ResetUsersTotals()
        {
            _usersCreated = 0;
            _usersFailed = 0;
        }

        public static string GetPassword()
        {
            return Password;
        }

        public static string GetUserEmailPrefix()
        {
            return UserEmailPrefix;
        }

        private static void WirteToCsv(string[] tokens, string[] tokensSecrets)
        {
            var theBuilder = new System.Text.StringBuilder();
            
            for (int i = 0; i < tokens.Length; i++)
            {
                theBuilder.Append(tokens[i]);
                theBuilder.Append(",");
                theBuilder.Append(tokensSecrets[i]);
                theBuilder.Append("\n");
            }

            using (var theWriter = new StreamWriter("CreatedUsers.csv"))
            {
                theWriter.Write(theBuilder.ToString());
            }
        }
    }
}