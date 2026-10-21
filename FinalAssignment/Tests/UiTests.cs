using FluentAssertions;
using ParaBank.Models;
using ParaBank.Pages;

namespace ParaBank.Tests
{
    [TestFixture]
    public class UiTests : BaseTest
    {
        [Test, Description("UI-1: User can be created")]
        public void UI1_UserCanBeCreated()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/index.htm");
            var homePage = new HomePage(Driver);

            var user = new CustomerModel();
            var registerPage = homePage.ClickRegister();
            var welcomePage = registerPage.FillAndSubmitRegistration(user);

            welcomePage.GetHeaderWelcomeText().Should().Be($"Welcome {user.Username}");
            welcomePage.GetLeftNavWelcomeText().Should().Be($"Welcome {user.FirstName} {user.LastName}");
        }

        [Test, Description("UI-2: User can transfer money between accounts")]
        public void UI2_UserCanTransferMoneyBetweenAccounts()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/index.htm");
            new HomePage(Driver).Login("john", "demo").GetLeftNavWelcomeText();
            
            Driver.Navigate().GoToUrl($"{BaseUrl}/transfer.htm");
            var transferPage = new TransferPage(Driver);

            transferPage.GetPageTitle().Should().Be("Transfer Funds");
            transferPage.TransferFunds("100.00");

            Driver.Navigate().GoToUrl($"{BaseUrl}/overview.htm");
            var overviewPage = new AccountsOverviewPage(Driver);
            overviewPage.Should().NotBeNull();
        }

        [Test, Description("UI-3: User can see details for each transaction in account")]
        public void UI3_UserCanSeeDetailsForEachTransaction()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/index.htm");
            new HomePage(Driver).Login("john", "demo").GetLeftNavWelcomeText();

            // Trigger transaction
            Driver.Navigate().GoToUrl($"{BaseUrl}/transfer.htm");
            new TransferPage(Driver).TransferFunds("50.00");

            Driver.Navigate().GoToUrl($"{BaseUrl}/overview.htm");
            var overviewPage = new AccountsOverviewPage(Driver);
            var transactionDetailsPage = overviewPage.OpenFirstAccountTransactions();

            transactionDetailsPage.ClickFirstTransaction();
            transactionDetailsPage.GetTransactionIdText().Should().NotBeNullOrEmpty();
        }
    }
}