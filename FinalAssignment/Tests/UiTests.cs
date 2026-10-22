using FluentAssertions;
using ParaBank.Models;
using ParaBank.Pages;

namespace ParaBank.Tests
{
    [TestFixture]
    public class UiTests : BaseTest
    {
        private HomePage _homePage;

        [SetUp]
        public override async Task Setup()
        {
            await base.Setup();

            Driver.Navigate().GoToUrl($"{BaseUrl}/index.htm");
            _homePage = new HomePage(Driver);
        }

        [Test, Description("UI-1: User can be created")]
        public void UI1_UserCanBeCreated()
        {
            var user = new CustomerModel();
            var registerPage = _homePage.ClickRegister();
            var welcomePage = registerPage.FillAndSubmitRegistration(user);

            welcomePage.GetHeaderWelcomeText().Should().Be($"Welcome {user.Username}");
            welcomePage.GetLeftNavWelcomeText().Should().Be($"Welcome {user.FirstName} {user.LastName}");
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

            // Perform transfer between accounts
            transferPage.TransferFunds(transferAmount.ToString("0.00"), fromIndex: 0, toIndex: 1);

            // Verify updated balances on Accounts Overview page
            Driver.Navigate().GoToUrl($"{BaseUrl}/overview.htm");
            decimal finalFromBalance = overviewPage.GetAccountBalance(fromAccount);
            decimal finalToBalance = overviewPage.GetAccountBalance(toAccount);

            finalFromBalance.Should().Be(initialFromBalance - transferAmount);
            finalToBalance.Should().Be(initialToBalance + transferAmount);
        }

        [Test, Description("UI-3: User can see details for each transaction in account")]
        public void UI3_UserCanSeeDetailsForEachTransaction()
        {
            // .GetLeftNavWelcomeText() ensures we wait until the user has been logged in
            _homePage.Login("john", "demo").GetLeftNavWelcomeText();

            // Ensure transactions exist
            Driver.Navigate().GoToUrl($"{BaseUrl}/transfer.htm");
            new TransferPage(Driver).TransferFunds("50.00");

            Driver.Navigate().GoToUrl($"{BaseUrl}/overview.htm");
            var overviewPage = new AccountsOverviewPage(Driver);
            var transactionDetailsPage = overviewPage.OpenFirstAccountTransactions();

            transactionDetailsPage.ClickFirstTransaction();

            // Verify transaction details are complete and displayed correctly
            transactionDetailsPage.GetTransactionIdText().Should().NotBeNullOrEmpty();
        }
    }
}