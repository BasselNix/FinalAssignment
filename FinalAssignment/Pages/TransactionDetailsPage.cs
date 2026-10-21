using OpenQA.Selenium;

namespace ParaBank.Pages
{
    public class TransactionDetailsPage : BasePage
    {
        private readonly By _firstTransactionLink = By.CssSelector("#transactionTable tbody tr td a");
        private readonly By _transactionId =
            By.XPath("//tr[td[contains(translate(normalize-space(.), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'transaction id')]]/td[2]");

        public TransactionDetailsPage(IWebDriver driver) : base(driver) { }

        public void ClickFirstTransaction()
        {
            Wait.Until(d => d.FindElements(_firstTransactionLink).Count > 0);
            Click(_firstTransactionLink);
        }

        public string GetTransactionIdText() => GetText(_transactionId);
    }
}