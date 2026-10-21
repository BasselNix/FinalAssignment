using OpenQA.Selenium;

namespace ParaBank.Pages
{
    public class HomePage : BasePage
    {
        private readonly By _registerLink = By.XPath("//a[text()='Register']");
        private readonly By _usernameInput = By.Name("username");
        private readonly By _passwordInput = By.Name("password");
        private readonly By _loginButton = By.CssSelector("input[value='Log In']");
        private readonly By _leftNavWelcome = By.ClassName("smallText");
        private readonly By _pageTitle = By.CssSelector("h1.title");

        public HomePage(IWebDriver driver) : base(driver) { }

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

        public string GetLeftNavWelcomeText()
        {
            // Explicitly wait until the paragraph element contains non-empty text
            return Wait.Until(d =>
            {
                var text = d.FindElement(_leftNavWelcome).Text;
                return !string.IsNullOrWhiteSpace(text) ? text.Trim() : null;
            })!;
        }

        public string GetHeaderWelcomeText() => GetText(_pageTitle).Trim();
    }
}