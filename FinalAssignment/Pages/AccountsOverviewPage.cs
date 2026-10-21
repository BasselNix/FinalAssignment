using OpenQA.Selenium;

namespace ParaBank.Pages
{
    public class AccountsOverviewPage : BasePage
    {
        private readonly By _accountLinks = By.CssSelector("#accountTable tbody tr td a");

        public AccountsOverviewPage(IWebDriver driver) : base(driver) { }

        public List<string> GetAccountNumbers()
        {
            Wait.Until(d => d.FindElements(_accountLinks).Count > 0);
            return Driver.FindElements(_accountLinks)
                         .Select(e => e.Text.Trim())
                         .Where(text => !string.IsNullOrEmpty(text))
                         .ToList();
        }

        public TransactionDetailsPage OpenFirstAccountTransactions()
        {
            Wait.Until(d => d.FindElements(_accountLinks).Count > 0);
            var links = Driver.FindElements(_accountLinks);
            links[0].Click();
            return new TransactionDetailsPage(Driver);
        }

        public decimal GetAccountBalance(string accountId)
        {
            var cellLocator = By.XPath($"//tr[td/a[text()='{accountId}']]/td[2]");
            var cellText = GetText(cellLocator);
            return decimal.Parse(cellText.Replace("$", "").Trim());
        }
    }
}