using OpenQA.Selenium;

namespace ParaBank.Pages
{
    public class TransactionDetailsPage : BasePage
    {
        private readonly By _firstTransactionLink = By.CssSelector("#transactionTable tbody tr td a");
        private readonly By _transactionId = By.XPath("//td[contains(.,'Transaction ID:')]/following-sibling::td");

        public TransactionDetailsPage(IWebDriver driver) : base(driver) { }

        public void ClickFirstTransaction()
        {
            Wait.Until(d => d.FindElements(_firstTransactionLink).Count > 0);
            Click(_firstTransactionLink);
        }

        public string GetTransactionIdText() => GetText(_transactionId);
    }
}