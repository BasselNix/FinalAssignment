using FluentAssertions;
using FluentAssertions.Execution;
using ParaBank.Models;
using ParaBank.Pages;

namespace ParaBank.Tests
{
    [TestFixture]
    public class UiTests : BaseTest
    {
        private HomePage _homePage;

        [SetUp]
        public async Task UiTestsSetup()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/index.htm");
            _homePage = new HomePage(Driver);
        }

        [Test, Description("UI-1: User can be created")]
        public void UI1_UserCanBeCreated()
        {
            var user = new CustomerModel();
            var registerPage = _homePage.ClickRegister();
            var welcomePage = registerPage.FillAndSubmitRegistration(user);

            using (new AssertionScope())
            {
                welcomePage.GetRegistrationSuccessMessage().Should().Be("Your account was created successfully. You are now logged in.");
                welcomePage.GetHeaderWelcomeText().Should().Be($"Welcome {user.Username}");
                welcomePage.GetLeftNavWelcomeText().Should().Be($"Welcome {user.FirstName} {user.LastName}");
            }
        }

        [Test, Description("UI-2: User can transfer money between accounts")]
        public void UI2_UserCanTransferMoneyBetweenAccounts()
        {
            // .GetLeftNavWelcomeText() ensures we wait until the user has been logged in
            _homePage.Login("john", "demo").GetLeftNavWelcomeText();

            Driver.Navigate().GoToUrl($"{BaseUrl}/overview.htm");
            var overviewPage = new AccountsOverviewPage(Driver);

            var accountNumbers = overviewPage.GetAccountNumbers();
            accountNumbers.Should().HaveCountGreaterThanOrEqualTo(2, "At least two accounts are required to perform a transfer.");

            var fromAccount = accountNumbers[0];
            var toAccount = accountNumbers[1];

            // Record initial balances for verification
            decimal initialFromBalance = overviewPage.GetAccountBalance(fromAccount);
            decimal initialToBalance = overviewPage.GetAccountBalance(toAccount);
            const decimal transferAmount = 100.00m;

            Driver.Navigate().GoToUrl($"{BaseUrl}/transfer.htm");
            var transferPage = new TransferPage(Driver);
            transferPage.GetPageTitle().Should().Be("Transfer Funds");

            var dropdownAccounts = transferPage.GetFromAccountDropdownOptions();
            dropdownAccounts.Should().BeEquivalentTo(accountNumbers, "Account numbers in the dropdown should belong to the logged-in user.");

            // Perform transfer between accounts
            transferPage.TransferFunds(transferAmount.ToString("0.00"), fromIndex: 0, toIndex: 1);

            transferPage.GetTransferSuccessTitle().Should().Be("Transfer Complete!", "Transfer should complete successfully and display completion confirmation.");

            // Verify updated balances on Accounts Overview page
            Driver.Navigate().GoToUrl($"{BaseUrl}/overview.htm");
            decimal finalFromBalance = overviewPage.GetAccountBalance(fromAccount);
            decimal finalToBalance = overviewPage.GetAccountBalance(toAccount);

            using (new AssertionScope())
            {
                finalFromBalance.Should().Be(initialFromBalance - transferAmount);
                finalToBalance.Should().Be(initialToBalance + transferAmount);
            }
        }

        [Test, Description("UI-3: User can see details for each transaction in account")]
        public void UI3_UserCanSeeDetailsForEachTransaction()
        {
            // .GetLeftNavWelcomeText() ensures we wait until the user has been logged in
            _homePage.Login("john", "demo").GetLeftNavWelcomeText();

            Driver.Navigate().GoToUrl($"{BaseUrl}/overview.htm");
            var overviewPage = new AccountsOverviewPage(Driver);
            var transactionDetailsPage = overviewPage.OpenFirstAccountTransactions();
            overviewPage.ClickFirstTransactionLink();

            var transactionExpectedDetails = new TransactionModel
            {
                Id = "12145",
                Date = "12-11-2025",
                Description = "Check # 1111",
                Type = "Credit",
                Amount = "$300.00"
            };
            var transactionActualDetails = transactionDetailsPage.GetTransactionDetails();

            transactionActualDetails.Should().BeEquivalentTo(transactionExpectedDetails);
        }
    }
}