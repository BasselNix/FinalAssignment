using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ParaBank.Pages
{
    public class TransferPage(IWebDriver driver) : BasePage(driver)
    {
        private static readonly By _amountInput = By.Id("amount");
        private static readonly By _fromAccountDropdown = By.Id("fromAccountId");
        private static readonly By _toAccountDropdown = By.Id("toAccountId");
        private static readonly By _transferBtn = By.XPath("//input[@value='Transfer']");
        private static readonly By _titleHeader = By.CssSelector("h1.title");

        public string GetPageTitle() => GetText(_titleHeader);

        public void TransferFunds(string amount, int fromIndex = 0, int toIndex = 1)
        {
            Type(_amountInput, amount);

            // Wait until options are populated in dropdowns using lambdas
            Wait.Until(d => new SelectElement(d.FindElement(_fromAccountDropdown)).Options.Count > fromIndex);
            var fromSelect = new SelectElement(Find(_fromAccountDropdown));
            fromSelect.SelectByIndex(fromIndex);
            Wait.Until(d => new SelectElement(d.FindElement(_toAccountDropdown)).Options.Count > toIndex);
            var toSelect = new SelectElement(Find(_toAccountDropdown));
            toSelect.SelectByIndex(toIndex);

            Click(_transferBtn);
        }
    }
}