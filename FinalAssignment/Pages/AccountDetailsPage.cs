using OpenQA.Selenium;
using ParaBank.Models;

namespace ParaBank.Pages
{
    public class AccountDetailsPage(IWebDriver driver) : BasePage(driver)
    {
        private static readonly By _transactionLinks = By.CssSelector("#transactionTable tbody tr td a");

        public TransactionDetailsPage ClickFirstTransaction()
        {
            Wait.Until(d => d.FindElements(_transactionLinks).Count > 0);
            var links = Driver.FindElements(_transactionLinks);
            links[0].Click();
            return new TransactionDetailsPage(Driver);
        }
    }
}