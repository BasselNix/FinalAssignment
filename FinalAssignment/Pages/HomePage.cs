using OpenQA.Selenium;

namespace ParaBank.Pages;

public class HomePage(IWebDriver driver) : BasePage(driver)
{
    private static readonly By _registerLink = By.XPath("//a[text()='Register']");
    private static readonly By _usernameInput = By.Name("username");
    private static readonly By _passwordInput = By.Name("password");
    private static readonly By _loginButton = By.CssSelector("input[value='Log In']");
    private static readonly By _leftNavWelcome = By.ClassName("smallText");
    private static readonly By _pageTitle = By.CssSelector("h1.title");
    private static readonly By _rightPanelParagraph = By.CssSelector("#rightPanel p");

    public RegisterPage ClickRegister()
    {
        Click(_registerLink);
        return new RegisterPage(Driver);
    }

    public HomePage Login(string username, string password)
    {
        Type(_usernameInput, username);
        Type(_passwordInput, password);
        Click(_loginButton);
        return this;
    }

    public string GetRegistrationSuccessMessage() => GetText(_rightPanelParagraph);

    public string GetLeftNavWelcomeText()
    {
        return Wait.Until(d =>
        {
            var text = d.FindElement(_leftNavWelcome).Text;
            return !string.IsNullOrWhiteSpace(text) ? text.Trim() : null;
        })!;
    }

    public string GetHeaderWelcomeText() => GetText(_pageTitle).Trim();
}