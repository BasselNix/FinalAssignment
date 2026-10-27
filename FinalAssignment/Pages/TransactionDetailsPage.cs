using OpenQA.Selenium;
using ParaBank.Models;

namespace ParaBank.Pages
{
    public class TransactionDetailsPage(IWebDriver driver) : BasePage(driver)
    {
        private static By GetTransactionInfoLocator(string infoTitle) =>
            By.XPath($"//tr[td[contains(translate(normalize-space(.), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '{infoTitle}')]]/td[2]");

        public TransactionModel GetTransactionDetails()
        {
            return new TransactionModel
            {
                Id = GetText(GetTransactionInfoLocator("id")),
                Date = GetText(GetTransactionInfoLocator("date")),
                Description = GetText(GetTransactionInfoLocator("description")),
                Type = GetText(GetTransactionInfoLocator("type")),
                Amount = GetText(GetTransactionInfoLocator("amount"))
            };
        }
    }
}