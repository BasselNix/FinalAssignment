using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ParaBank.Pages;

public class TransferPage(IWebDriver driver) : BasePage(driver)
{
    private static readonly By _amountInput = By.Id("amount");
    private static readonly By _fromAccountDropdown = By.Id("fromAccountId");
    private static readonly By _toAccountDropdown = By.Id("toAccountId");
    private static readonly By _transferBtn = By.XPath("//input[@value='Transfer']");
    private static readonly By _titleHeader = By.CssSelector("h1.title");
    private static readonly By _transferSuccessHeader = By.XPath("//h1[text()='Transfer Complete!']");

    public string GetPageTitle() => GetText(_titleHeader);

    public List<string> GetFromAccountDropdownOptions()
    {
        Wait.Until(d => new SelectElement(d.FindElement(_fromAccountDropdown)).Options.Count > 0);
        var selectElement = new SelectElement(Find(_fromAccountDropdown));
        return selectElement.Options.Select(o => o.Text.Trim()).ToList();
    }

    public void TransferFunds(string amount, int fromIndex = 0, int toIndex = 1)
    {
        Type(_amountInput, amount);

        Wait.Until(d => new SelectElement(d.FindElement(_fromAccountDropdown)).Options.Count > fromIndex);
        var fromSelect = new SelectElement(Find(_fromAccountDropdown));
        fromSelect.SelectByIndex(fromIndex);
        Wait.Until(d => new SelectElement(d.FindElement(_toAccountDropdown)).Options.Count > toIndex);
        var toSelect = new SelectElement(Find(_toAccountDropdown));
        toSelect.SelectByIndex(toIndex);

        Click(_transferBtn);
    }

    public string GetTransferSuccessTitle() => GetText(_transferSuccessHeader);
}