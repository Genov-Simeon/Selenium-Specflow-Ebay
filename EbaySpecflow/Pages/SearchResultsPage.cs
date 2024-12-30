using OpenQA.Selenium;
using System.Text.RegularExpressions;

namespace EbayMonopolyTests.Pages
{
    public class SearchResultsPage
    {
        private readonly IWebDriver _driver;
        private decimal _firstItemPrice;
        public decimal SavedPrice => _firstItemPrice;

        public SearchResultsPage(IWebDriver driver)
        {
            _driver = driver;
        }

        private IWebElement FirstItemTitle => _driver.FindElement(By.CssSelector(".srp-results .s-item__title"));
        private IWebElement FirstItemPrice => _driver.FindElement(By.CssSelector(".srp-results .s-item__price"));
        private IWebElement ShippingInfo => _driver.FindElement(By.CssSelector("div.srp-controls__row-cells.right span.s-zipcode-entry__label"));

        public string GetFirstItemTitle()
        {
            return FirstItemTitle.Text;
        }

        public bool IsShippingToBulgariaAvailable()
        {
            return ShippingInfo.Text.Contains("Bulgaria");
        }

        public decimal GetFirstItemPrice()
        {
            Match match = Regex.Match(FirstItemPrice.Text, @"\$(\d+\.\d+)");
            _firstItemPrice = decimal.Parse(match.Groups[1].Value);
            return _firstItemPrice;
        }

        public void ClickFirstItem()
        {
            FirstItemTitle.Click();
        }
    }
} 