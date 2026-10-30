using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ParaBank.Utilities;

namespace ParaBank.Pages;

public abstract class BasePage
{
    protected IWebDriver Driver;
    protected WebDriverWait Wait;

    protected BasePage(IWebDriver driver)
    {
        Driver = driver;
        Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }

    protected IWebElement Find(By locator) =>
        Wait.Until(d =>
        {
            var element = d.FindElement(locator);
            return element.Displayed ? element : null;
        })!;

    protected void Click(By locator)
    {
        Logger.Info($"Clicking on element: {locator}");
        try
        {
            Wait.Until(d =>
            {
                var element = d.FindElement(locator);
                return element.Displayed && element.Enabled ? element : null;
            }).Click();
        }
        catch (WebDriverTimeoutException ex)
        {
            throw new WebDriverTimeoutException($"Element located by '{locator}' is not visible, enabled, or clickable.", ex);
        }
    }

    protected void Type(By locator, string text)
    {
        Logger.Info($"Typing '{text}' into field: {locator}");
        try
        {
            var element = Wait.Until(d =>
            {
                var element = d.FindElement(locator);
                return element.Displayed && element.Enabled ? element : null;
            });

            element!.Clear();
            element.SendKeys(text);
        }
        catch (WebDriverTimeoutException ex)
        {
            throw new WebDriverTimeoutException($"Element located by '{locator}' is not visible, enabled, or clickable.", ex);
        }
    }

    protected string GetText(By locator)
    {
        try
        {
            return Wait.Until(d =>
            {
                var element = d.FindElement(locator);
                return element.Displayed && element.Enabled ? element : null;
            }).Text;
        }
        catch (WebDriverTimeoutException ex)
        {
            throw new WebDriverTimeoutException($"Element located by '{locator}' is not visible, enabled, or clickable.", ex);
        }
    }
}