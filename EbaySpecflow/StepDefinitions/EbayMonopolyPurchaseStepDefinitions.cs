using TechTalk.SpecFlow;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using EbayMonopolyTests.Pages;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Collections.ObjectModel;

namespace EbayMonopolyTests.StepDefinitions
{
    [Binding]
    public class EbayMonopolyPurchaseStepDefinitions
    {
        private readonly IWebDriver _driver;
        private readonly HomePage _homePage;
        private readonly SearchResultsPage _searchResultsPage;
        private readonly ItemDetailsPage _itemDetailsPage;
        private readonly CartPage _cartPage;
        
        public EbayMonopolyPurchaseStepDefinitions()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            
            _homePage = new HomePage(_driver);
            _searchResultsPage = new SearchResultsPage(_driver);
            _itemDetailsPage = new ItemDetailsPage(_driver);
            _cartPage = new CartPage(_driver);
        }

        [Given(@"I navigate to eBay homepage")]
        public void GivenINavigateToEBayHomepage()
        {
            _homePage.NavigateToHomePage();
        }

        [When(@"I select ""(.*)"" from the category dropdown")]
        public void WhenISelectFromTheCategoryDropdown(string category)
        {
            _homePage.SelectCategory(category);
        }

        [When(@"I search for ""(.*)""")]
        public void WhenISearchFor(string item)
        {
            _homePage.SearchForItem(item);
        }

        [Then(@"I should see ""(.*)"" in the first item title")]
        public void ThenIShouldSeeInTheFirstItemTitle(string expectedTitle)
        {
            Assert.That(_searchResultsPage.GetFirstItemTitle(), Does.Contain(expectedTitle).IgnoreCase);
        }

        [Then(@"shipping to Bulgaria should be available")]
        public void ThenShippingToBulgariaShouldBeAvailable()
        {
            Assert.That(_searchResultsPage.IsShippingToBulgariaAvailable(), Is.True);
        }

        [Then(@"the item price should be displayed")]
        public void ThenTheItemPriceShouldBeDisplayed()
        {
            Assert.That(_searchResultsPage.GetFirstItemPrice(), Is.GreaterThan(0));
        }

        [When(@"I click on the first item")]
        public void WhenIClickOnTheFirstItem()
        {
            string currentWindow = _driver.CurrentWindowHandle;

            _searchResultsPage.ClickFirstItem();

            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(driver => driver.WindowHandles.Count > 1);

            var allWindows = _driver.WindowHandles;

            foreach (var window in allWindows)
            {
                if (window != currentWindow)
                {
                    _driver.SwitchTo().Window(window);
                    break;
                }
            }
        }

        [Then(@"the details page title should contain ""(.*)""")]
        public void ThenTheDetailsPageTitleShouldContain(string expectedTitle)
        {
            Assert.That(_itemDetailsPage.GetItemTitle(), Does.Contain(expectedTitle).IgnoreCase);
        }

        [Then(@"the price should match the search page price")]
        public void ThenThePriceShouldMatchTheSearchPagePrice()
        {
            Assert.That(_itemDetailsPage.GetItemPrice(), Is.EqualTo(_searchResultsPage.SavedPrice));
        }

        [When(@"When I open the shipping details")]
        public void WhenIOpenShippingDetails()
        {
            _itemDetailsPage.OpenShippingDetails();
        }

        [Then(@"Bulgaria should be available in the shipping countries")]
        public void ThenBulgariaShouldBeAvailableInTheShippingCountries()
        {
            Assert.That(_itemDetailsPage.IsCountryAvailable("Bulgaria"), Is.True);
            _itemDetailsPage.CloseShippingDetailsWindow();
            new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("ux-overlay")));
        }

        [When(@"I select quantity ""(.*)""")]
        public void WhenISelectQuantity(string quantity)
        {
            IWebElement dropdownButton = _driver.FindElement(By.ClassName("listbox-button__control"));
            dropdownButton.Click();

            ReadOnlyCollection<IWebElement> allOptions = _driver.FindElements(By.CssSelector(".listbox__options > .listbox__option:not([aria-disabled])"));
            allOptions[3].Click();

            _itemDetailsPage.GetItemPrice();

            _itemDetailsPage.SelectQuantity(quantity);
        }

        [When(@"I click Add to cart")]
        public void WhenIClickAddToCart()
        {
            _itemDetailsPage.ClickAddToCart();
        }

        [Then(@"I should be on the cart page")]
        public void ThenIShouldBeOnTheCartPage()
        {
            Assert.That(_cartPage.GetCurrentUrl(), Does.Contain("https://cart.payments.ebay.com/"));
        }

        [Then(@"the quantity should be ""(.*)""")]
        public void ThenTheQuantityShouldBe(string expectedQuantity)
        {
            Assert.That(_cartPage.GetSelectedQuantity(), Is.EqualTo(expectedQuantity));
        }

        [Then(@"the price should be updated for (.*) items")]
        public void ThenThePriceShouldBeUpdatedForItems(int quantity)
        {
            var expectedTotal = _itemDetailsPage.SavedPrice * quantity;
            Assert.That(_cartPage.GetTotalPrice(), Is.EqualTo(expectedTotal));
        }

        [AfterScenario]
        public void AfterScenario()
        {
            _driver.Quit();
        }
    }
} 