using OpenQA.Selenium;

namespace ParaBank.Pages
{
    public class TransactionDetailsPage(IWebDriver driver) : BasePage(driver)
    {
        private static readonly By _firstTransactionLink = By.CssSelector("#transactionTable tbody tr td a");
        private static By GetTransactionInfoLocator(string infoTitle) =>
            By.XPath($"//tr[td[contains(translate(normalize-space(.), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '{infoTitle}')]]/td[2]");

        public void ClickFirstTransaction()
        {
            Wait.Until(d => d.FindElements(_firstTransactionLink).Count > 0);
            Click(_firstTransactionLink);
        }

        public string GetTransactionInfoId() => GetText(GetTransactionInfoLocator("id"));
        public string GetTransactionInfoDate() => GetText(GetTransactionInfoLocator("date"));
        public string GetTransactionInfoDescription() => GetText(GetTransactionInfoLocator("description"));
        public string GetTransactionInfoType() => GetText(GetTransactionInfoLocator("type"));
        public string GetTransactionInfoAmount() => GetText(GetTransactionInfoLocator("amount"));
    }
}