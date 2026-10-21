using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ParaBank.Utilities;
using RestSharp;

namespace ParaBank.Tests
{
    public abstract class BaseTest
    {
        protected IWebDriver Driver = null!;
        protected RestClient RestClient = null!;

        protected const string BaseUrl = "http://localhost:8080/parabank";
        protected const string ApiBaseUrl = "http://localhost:8080/parabank/services/bank";

        [SetUp]
        public async Task Setup()
        {
            Logger.Info("Initializing RestSharp API Client...");
            RestClient = new RestClient(ApiBaseUrl);

            await ResetDatabase();

            Logger.Info("Initializing Chrome Browser Driver...");
            var options = new ChromeOptions();
            // Remove if you prefer a visible browser window
            options.AddArgument("--headless=new");

            Driver = new ChromeDriver(options);
            Driver.Manage().Window.Maximize();
        }

        private async Task ResetDatabase()
        {
            Logger.Info("Cleaning Database via API...");
            var cleanDbReq = new RestRequest("cleanDB", Method.Post);
            var cleanResp = await RestClient.PostAsync(cleanDbReq);
            Logger.Info($"cleanDB Response Status: {cleanResp.StatusCode}");

            Logger.Info("Initializing Database via API...");
            var initDbReq = new RestRequest("initializeDB", Method.Post);
            var initResp = await RestClient.PostAsync(initDbReq);
            Logger.Info($"initializeDB Response Status: {initResp.StatusCode}");
        }

        [TearDown]
        public void TearDown()
        {
            if (Driver != null)
            {
                Logger.Info("Quitting Chrome Driver...");
                Driver.Quit();
                Driver.Dispose();
            }

            if (RestClient != null)
            {
                Logger.Info("Disposing RestSharp Client...");
                RestClient.Dispose();
            }
        }
    }
}