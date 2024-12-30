using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace EbayMonopolyTests.Pages
{
    public class HomePage
    {
        private readonly IWebDriver _driver;
        
        public HomePage(IWebDriver driver)
        {
            _driver = driver;
        }

        private IWebElement CategoryDropdown => _driver.FindElement(By.Id("gh-cat"));
        private IWebElement SearchBox => _driver.FindElement(By.Id("gh-ac"));
        private IWebElement SearchButton => _driver.FindElement(By.Id("gh-btn"));

        public void NavigateToHomePage()
        {
            _driver.Navigate().GoToUrl("https://ebay.com");
        }

        public void SelectCategory(string category)
        {
            var select = new SelectElement(CategoryDropdown);
            select.SelectByText(category);
        }

        public void SearchForItem(string item)
        {
            SearchBox.SendKeys(item);
            SearchButton.Click();
        }
    }
} 