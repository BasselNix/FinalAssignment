using OpenQA.Selenium;
using ParaBank.Models;

namespace ParaBank.Pages
{
    public class RegisterPage : BasePage
    {
        private readonly By _firstName = By.Id("customer.firstName");
        private readonly By _lastName = By.Id("customer.lastName");
        private readonly By _street = By.Id("customer.address.street");
        private readonly By _city = By.Id("customer.address.city");
        private readonly By _state = By.Id("customer.address.state");
        private readonly By _zipCode = By.Id("customer.address.zipCode");
        private readonly By _phone = By.Id("customer.phoneNumber");
        private readonly By _ssn = By.Id("customer.ssn");
        private readonly By _username = By.Id("customer.username");
        private readonly By _password = By.Id("customer.password");
        private readonly By _confirmPassword = By.Id("repeatedPassword");
        private readonly By _registerButton = By.XPath("//input[@value='Register']");

        public RegisterPage(IWebDriver driver) : base(driver) { }

        public HomePage FillAndSubmitRegistration(CustomerModel user)
        {
            Type(_firstName, user.FirstName);
            Type(_lastName, user.LastName);
            Type(_street, user.Address.Street);
            Type(_city, user.Address.City);
            Type(_state, user.Address.State);
            Type(_zipCode, user.Address.ZipCode);
            Type(_phone, user.PhoneNumber);
            Type(_ssn, user.Ssn);
            Type(_username, user.Username);
            Type(_password, user.Password);
            Type(_confirmPassword, user.Password);

            Click(_registerButton);
            return new HomePage(Driver);
        }
    }
}