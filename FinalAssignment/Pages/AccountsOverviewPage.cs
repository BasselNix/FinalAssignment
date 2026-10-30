using OpenQA.Selenium;
using System.Globalization;

namespace ParaBank.Pages;

public class AccountsOverviewPage(IWebDriver driver) : BasePage(driver)
{
    private static readonly By _accountLinks = By.CssSelector("#accountTable tbody tr td a");
    private static By GetAccountBalanceCellLocator(string accountId) => By.XPath($"//tr[td/a[text()='{accountId}']]/td[2]");

    public List<string> GetAccountNumbers()
    {
        Wait.Until(d => d.FindElements(_accountLinks).Count > 0);
        return Driver.FindElements(_accountLinks)
            .Select(e => e.Text.Trim())
            .Where(text => !string.IsNullOrEmpty(text))
            .ToList();
    }

    public AccountDetailsPage OpenFirstAccountDetails()
    {
        Wait.Until(d => d.FindElements(_accountLinks).Count > 0);
        var links = Driver.FindElements(_accountLinks);
        links[0].Click();
        return new AccountDetailsPage(Driver);
    }

    public decimal GetAccountBalance(string accountId)
    {
        var cellLocator = GetAccountBalanceCellLocator(accountId);
        var cellText = GetText(cellLocator);
        return decimal.Parse(cellText.Replace("$", "").Trim(), CultureInfo.InvariantCulture);
    }
}