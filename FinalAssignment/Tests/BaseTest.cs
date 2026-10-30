using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ParaBank.Utilities;
using RestSharp;
using RestSharp.Serializers.Json;
using System.Text.Json.Serialization;

namespace ParaBank.Tests;

public abstract class BaseTest
{
    protected IWebDriver Driver;
    protected RestClient RestClient;

    protected const string BaseUrl = "http://localhost:8080/parabank";
    protected const string ApiBaseUrl = "http://localhost:8080/parabank/services/bank";

    [SetUp]
    public async Task Setup()
    {
        Logger.Info("Initializing RestSharp API Client...");
        var restClientOptions = new RestClientOptions(ApiBaseUrl);
        RestClient = new RestClient(restClientOptions, configureSerialization: s =>
            s.UseSystemTextJson(new System.Text.Json.JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() },
                PropertyNameCaseInsensitive = true
            })
        );

        await ResetDatabase();

        Logger.Info("Initializing Chrome Browser Driver...");
        var chromeOptions = new ChromeOptions();
        chromeOptions.AddArgument("--headless=new");

        Driver = new ChromeDriver(chromeOptions);
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
            Driver.Dispose();
        }

        if (RestClient != null)
        {
            Logger.Info("Disposing RestSharp Client...");
            RestClient.Dispose();
        }
    }
}