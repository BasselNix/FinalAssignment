using FluentAssertions;
using FluentAssertions.Execution;
using ParaBank.Models;
using ParaBank.Pages;
using ParaBank.Utilities;
using RestSharp;

namespace ParaBank.Tests;

[TestFixture]
public class ApiTests : BaseTest
{
    [Test, Description("API-1: Edit User's data via API")]
    public async Task API1_EditUserDataViaApi()
    {
        const int targetUserId = 12323;
        var updateEndpoint = $"customers/update/{targetUserId}";

        var request = new RestRequest(updateEndpoint, Method.Post);
        request.AddQueryParameter("firstName", "UpdatedJohn");
        request.AddQueryParameter("lastName", "UpdatedDoe");
        request.AddQueryParameter("street", "999 Automation Way");
        request.AddQueryParameter("city", "TechCity");
        request.AddQueryParameter("state", "CA");
        request.AddQueryParameter("zipCode", "90210");
        request.AddQueryParameter("phoneNumber", "1234567890");
        request.AddQueryParameter("ssn", "999-99-9999");
        request.AddQueryParameter("username", "parasoft");
        request.AddQueryParameter("password", "demo");

        Logger.Info($"Sending POST to update customer {targetUserId}");
        var response = await RestClient.PostAsync(request);
        response.IsSuccessStatusCode.Should().BeTrue();

        var getRequest = new RestRequest($"customers/{targetUserId}", Method.Get);
        var customerData = await RestClient.GetAsync<CustomerModel>(getRequest);

        var correctData = new CustomerModel
        {
            FirstName = "UpdatedJohn",
            LastName = "UpdatedDoe",
            Address = new AddressModel
            {
                Street = "999 Automation Way",
                City = "TechCity",
                State = "CA",
                ZipCode = "90210"
            },
            PhoneNumber = "1234567890",
            Ssn = "999-99-9999",
        };

        using (new AssertionScope())
        {
            customerData.Should().NotBeNull();
            customerData.Should().BeEquivalentTo(correctData,
                options => options
                    .Excluding(ctx => ctx.Id)
                    .Excluding(ctx => ctx.Username));
        }
    }

    [Test, Description("API-2: User has an account after creation")]
    public async Task API2_UserHasAccountAfterCreation()
    {
        Driver.Navigate().GoToUrl($"{BaseUrl}/index.htm");
        var homePage = new HomePage(Driver);
        var newUser = new CustomerModel
        {
            FirstName = "Tester",
            LastName = "Testeroo",
            PhoneNumber = "555-0199",
            Ssn = "000-00-0000",
            Username = $"user_{Guid.NewGuid().ToString()[..8]}",
            Password = "Password123!"
        };

        homePage.ClickRegister().FillAndSubmitRegistration(newUser);

        var loginRequest = new RestRequest($"login/{newUser.Username}/{newUser.Password}", Method.Get);
        var customerData = await RestClient.GetAsync<CustomerModel>(loginRequest);

        customerData.Should().NotBeNull("Newly registered user should be reachable via API");
        int newCustomerId = customerData!.Id;

        Logger.Info($"Fetching accounts for newly registered customer ID {newCustomerId}");

        var request = new RestRequest($"customers/{newCustomerId}/accounts", Method.Get);
        var accounts = await RestClient.GetAsync<List<AccountModel>>(request);

        using (new AssertionScope())
        {
            accounts.Should().NotBeNull();
            accounts.Should().HaveCount(1, "A newly registered user should have exactly one bank account opened by default.");
            accounts[0].Balance.Should().Be(515.50m, "Default opening balance should match Admin Page configuration.");
        }
    }

    [Test, Description("API-3: Send funds via API")]
    public async Task API3_SendFundsViaApi()
    {
        const int fromAccountId = 13455;
        const int toAccountId = 12900;
        const decimal amount = 50.00m;

        var initialFrom = await RestClient.GetAsync<AccountModel>(new RestRequest($"accounts/{fromAccountId}", Method.Get));
        var initialTo = await RestClient.GetAsync<AccountModel>(new RestRequest($"accounts/{toAccountId}", Method.Get));

        var transferRequest = new RestRequest("transfer", Method.Post);
        transferRequest.AddQueryParameter("fromAccountId", fromAccountId);
        transferRequest.AddQueryParameter("toAccountId", toAccountId);
        transferRequest.AddQueryParameter("amount", amount);

        Logger.Info($"Transferring {amount} from {fromAccountId} to {toAccountId}");
        var transferResponse = await RestClient.PostAsync(transferRequest);
        transferResponse.IsSuccessStatusCode.Should().BeTrue();

        var updatedFrom = await RestClient.GetAsync<AccountModel>(new RestRequest($"accounts/{fromAccountId}", Method.Get));
        var updatedTo = await RestClient.GetAsync<AccountModel>(new RestRequest($"accounts/{toAccountId}", Method.Get));

        using (new AssertionScope())
        {
            updatedFrom!.Balance.Should().Be(initialFrom!.Balance - amount);
            updatedTo!.Balance.Should().Be(initialTo!.Balance + amount);
        }
    }
}