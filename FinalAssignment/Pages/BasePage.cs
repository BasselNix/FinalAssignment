using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ParaBank.Utilities;

namespace ParaBank.Pages
{
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
                    try
                    {
                        var element = d.FindElement(locator);
                        return element.Displayed && element.Enabled ? element : null;
                    }
                    catch (NoSuchElementException)
                    {
                        return null;
                    }
                })?.Click();
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new WebDriverTimeoutException($"Element located by '{locator}' is not visible, enabled, or clickable.", ex);
            }
        }

        protected void Type(By locator, string text)
        {
            Logger.Info($"Typing '{text}' into field: {locator}");
            Wait.Until(d =>
            {
                try
                {
                    var element = d.FindElement(locator);
                    if (element.Displayed && element.Enabled)
                    {
                        element.Clear();
                        element.SendKeys(text);
                        return true;
                    }
                    return false;
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
            });
        }

        protected string GetText(By locator) =>
            Wait.Until(d =>
            {
                try
                {
                    var text = d.FindElement(locator).Text;
                    return !string.IsNullOrWhiteSpace(text) ? text.Trim() : null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            })!;
    }
}