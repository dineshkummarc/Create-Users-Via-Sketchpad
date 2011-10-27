using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.Text.RegularExpressions;

namespace CreateUsers
{
    public class Sketchpad
    {
        public IWebDriver Driver;

        public void Setup()
        {
            Driver = new FirefoxDriver();
        }
        
        public void Teardown()
        {
            Driver.Quit();
        }
        
        public bool PopulateAndSignSketchpad(string url, string consumerKey, string consumerSecret)
        {
            try
            {
                IWebElement urlBox = Driver.FindElement(By.XPath(".//*[@id='apiurl']"));
                if (urlBox != null) urlBox.SendKeys(url);

                IWebElement consumerKeyBox = Driver.FindElement(By.XPath(".//*[@id='form1']/div/input[2]"));
                if (consumerKeyBox != null) consumerKeyBox.SendKeys(consumerKey);

                IWebElement consumerSecretBox = Driver.FindElement(By.XPath(".//*[@id='form1']/div/input[3]"));
                if (consumerSecretBox != null) consumerSecretBox.SendKeys(consumerSecret);

                IWebElement signRequestButton = Driver.FindElement(By.XPath(".//*[@id='form1']/div/input[6]"));
                if (signRequestButton != null) signRequestButton.Click();

                IWebElement openSignedURL = Driver.FindElement(By.XPath(".//*[@id='form1']/div/a[2]"));
                if (openSignedURL != null) openSignedURL.Click();

                return true;
            }
            catch (NoSuchElementException exception)
            {
                Console.WriteLine("Failed to create user: " + exception.Message);
                CreateUsers.UpdateUsersFailed();
                return false;
            }
        }
        
        public bool PopulateAndSignSketchpad(string url, string consumerKey, string consumerSecret, string tokenKey,
                                             string tokenSecret)
        {
            try
            {
                IWebElement urlBox = Driver.FindElement(By.XPath(".//*[@id='apiurl']"));
                if (urlBox != null) urlBox.SendKeys(url);

                IWebElement consumerKeyBox = Driver.FindElement(By.XPath(".//*[@id='form1']/div/input[2]"));
                if (consumerKeyBox != null) consumerKeyBox.SendKeys(consumerKey);

                IWebElement consumerSecretBox = Driver.FindElement(By.XPath(".//*[@id='form1']/div/input[3]"));
                if (consumerSecretBox != null) consumerSecretBox.SendKeys(consumerSecret);

                IWebElement tokenKeyBox = Driver.FindElement(By.XPath(".//*[@id='form1']/div/input[4]"));
                if (tokenKeyBox != null) tokenKeyBox.SendKeys(tokenKey);

                IWebElement tokensecretBox = Driver.FindElement(By.XPath(".//*[@id='form1']/div/input[5]"));
                if (tokensecretBox != null) tokensecretBox.SendKeys(tokenSecret);

                IWebElement signRequestButton = Driver.FindElement(By.XPath(".//*[@id='form1']/div/input[6]"));
                if (signRequestButton != null) signRequestButton.Click();

                IWebElement openSignedURL = Driver.FindElement(By.XPath(".//*[@id='form1']/div/a[2]"));
                if (openSignedURL != null) openSignedURL.Click();

                return true;
            }
            catch (NoSuchElementException exception)
            {
                Console.WriteLine("Failed to create user: " + exception.Message);
                CreateUsers.UpdateUsersFailed();
                return false;
            }
        }
        
        public bool UserAuthentication(String url)
        {
            try
            {
                Driver.Navigate().GoToUrl(url);


                IWebElement createEmail = Driver.FindElement(By.XPath(".//*[@id='SignUpData_Email']"));
                if (createEmail != null)createEmail.SendKeys(CreateUsers.GetUserEmailPrefix() + Guid.NewGuid().ToString() + "@7digital.com");
                   
                IWebElement createPassword = Driver.FindElement(By.XPath(".//*[@id='SignUpData_Password']"));
                if (createPassword != null) createPassword.SendKeys(CreateUsers.GetPassword());

                IWebElement confirmPassword = Driver.FindElement(By.XPath(".//*[@id='SignUpData_PasswordConfirm']"));
                if (confirmPassword != null) confirmPassword.SendKeys("test");

                IWebElement agreeToTermsAndConditions =
                    Driver.FindElement(By.XPath(".//*[@id='content']/form[2]/fieldset/label[4]/input"));
                if (agreeToTermsAndConditions != null) agreeToTermsAndConditions.Click();

                IWebElement signMeUpButton = Driver.FindElement(By.XPath(".//*[@id='SignUp.Submit']"));
                if (signMeUpButton != null) signMeUpButton.Click();

                IWebElement allowButton =
                    Driver.FindElement(By.XPath(".//*[@id='content']/ul/li[1]/form/fieldset/button"));
                if (allowButton != null) allowButton.Click();
                return true;
            }
            catch (NoSuchElementException exception)
            {
                Console.WriteLine("Failed to create user: " + exception.Message);
                CreateUsers.UpdateUsersFailed();
                return false;
            }
        }

        public String ExtractOauthToken()
        {
            try
            {
                const string oauthTokenPattern = "<oauth_token>(.*?)</oauth_token>";
                var oauthTokenRegex = new Regex(oauthTokenPattern);
                string oauthToken = oauthTokenRegex.Match(Driver.PageSource).Groups[1].Value;

                return oauthToken;
            }
            catch (NoSuchElementException exception)
            {
                Console.WriteLine("Failed to create user: " + exception.Message);
                CreateUsers.UpdateUsersFailed();
                return null;
            }
        }

        public String ExtractOauthTokenSecret()
        {
            try
            {

                const string oauthTokenSecretPattern = "<oauth_token_secret>(.*?)</oauth_token_secret>";
                var oauthTokenSecretRegex = new Regex(oauthTokenSecretPattern);
                string oauthTokenSecret = oauthTokenSecretRegex.Match(Driver.PageSource).Groups[1].Value;

                return oauthTokenSecret;
            }
            catch (NoSuchElementException exception)
            {
                Console.WriteLine("Failed to create user: " + exception.Message);
                CreateUsers.UpdateUsersFailed();
                return null;
            }
        }

        public void PopulateAndSignSketchpad ()
        {
            throw new NotImplementedException();
        }
    }
}