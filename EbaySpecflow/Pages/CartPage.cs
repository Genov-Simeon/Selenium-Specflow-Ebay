using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Globalization;
using System.Text.RegularExpressions;

namespace EbayMonopolyTests.Pages
{
    public class CartPage
    {
        private readonly IWebDriver _driver;

        public CartPage(IWebDriver driver)
        {
            _driver = driver;
        }

        private IWebElement QuantityDropdown => _driver.FindElement(By.CssSelector("div.grid-item-quantity select[data-test-id=\"qty-dropdown\"]"));
        private IWebElement TotalPrice => _driver.FindElement(By.CssSelector("div[data-test-id = \"SUBTOTAL\"] > span > span > span"));

        public string GetCurrentUrl()
        {
            return _driver.Url;
        }

        public string GetSelectedQuantity()
        {
            var select = new SelectElement(QuantityDropdown);
            return select.SelectedOption.Text;
        }

        public decimal GetTotalPrice()
        {
            Match match = Regex.Match(TotalPrice.Text, @"\d+\.\d{2}");
            var price = decimal.Parse(match.Value, CultureInfo.InvariantCulture);

            return price;
        }
    }
} 