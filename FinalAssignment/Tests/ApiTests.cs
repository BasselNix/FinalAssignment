using FluentAssertions;
using NUnit.Framework.Internal;
using ParaBank.Models;
using RestSharp;
using Logger = ParaBank.Utilities.Logger;

namespace ParaBank.Tests
{
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

            customerData.Should().NotBeNull();
            customerData!.FirstName.Should().Be("UpdatedJohn");
            customerData.LastName.Should().Be("UpdatedDoe");
        }

        [Test, Description("API-2: User has an account after creation")]
        public async Task API2_UserHasAccountAfterCreation()
        {
            const int customerId = 12323;
            var request = new RestRequest($"customers/{customerId}/accounts", Method.Get);

            Logger.Info($"Fetching accounts for customer {customerId}");
            var accounts = await RestClient.GetAsync<List<AccountModel>>(request);

            accounts.Should().NotBeNull();
            accounts.Should().NotBeEmpty();
            accounts![0].Balance.Should().BeGreaterThan(0);
        }

        [Test, Description("API-3: Send funds via API")]
        public async Task API3_SendFundsViaApi()
        {
            const int fromAccountId = 13455;
            const int toAccountId = 12900;
            const decimal amount = 50.00m;

            var transferRequest = new RestRequest("transfer", Method.Post);
            transferRequest.AddQueryParameter("fromAccountId", fromAccountId);
            transferRequest.AddQueryParameter("toAccountId", toAccountId);
            transferRequest.AddQueryParameter("amount", amount);

            Logger.Info($"Transferring {amount} from {fromAccountId} to {toAccountId}");
            var transferResponse = await RestClient.PostAsync(transferRequest);
            transferResponse.IsSuccessStatusCode.Should().BeTrue();

            var fromAccountReq = new RestRequest($"accounts/{fromAccountId}", Method.Get);
            var accountDetails = await RestClient.GetAsync<AccountModel>(fromAccountReq);
            accountDetails.Should().NotBeNull();
        }
    }
}