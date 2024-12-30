using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Globalization;
using System.Text.RegularExpressions;

namespace EbayMonopolyTests.Pages
{
    public class ItemDetailsPage
    {
        private readonly IWebDriver _driver;
        private decimal _itemPrice;
        public decimal SavedPrice => _itemPrice;

        public ItemDetailsPage(IWebDriver driver)
        {
            _driver = driver;
        }

        private IWebElement ItemTitle => _driver.FindElement(By.CssSelector("h1.x-item-title__mainTitle span"));
        private IWebElement ItemPrice => _driver.FindElement(By.CssSelector("div.x-price-primary span"));
        private IWebElement ShippingDetails => _driver.FindElement(By.XPath("//button[child::span[text()='See details' and child::span[text()='for shipping']]]"));
        private IWebElement CloseShippingDetails => _driver.FindElement(By.CssSelector("button[aria-label=\"Close window\"]"));
        private IWebElement CountryDropdown => _driver.FindElement(By.Id("shCountry"));
        private IWebElement QuantityInput => _driver.FindElement(By.Id("qtyTextBox"));
        private IWebElement AddToCartButton => _driver.FindElement(By.Id("atcBtn_btn_1"));

        public string GetItemTitle()
        {
            return ItemTitle.Text;
        }

        public decimal GetItemPrice()
        {
            Match match = Regex.Match(ItemPrice.Text, @"\d+\.\d{2}");
            _itemPrice = decimal.Parse(match.Value, CultureInfo.InvariantCulture);

            return _itemPrice;
        }

        public void OpenShippingDetails()
        {
            ShippingDetails.Click();
        }

        public void CloseShippingDetailsWindow()
        {
            CloseShippingDetails.Click();
        }

        public bool IsCountryAvailable(string country)
        {
            var select = new SelectElement(CountryDropdown);
            return select.Options.Any(o => o.Text.Contains(country));
        }

        public void SelectQuantity(string quantity)
        {
            QuantityInput.Clear();
            QuantityInput.SendKeys(quantity);
        }

        public void ClickAddToCart()
        {
            AddToCartButton.Click();
        }
    }
} 