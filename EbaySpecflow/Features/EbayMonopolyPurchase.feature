@binding_source_valid
Feature: Ebay Monopoly Purchase
    As a user
    I want to search and purchase Monopoly game on eBay
    So that I can buy the board game

Scenario: Search, verify and add Monopoly game to cart
    Given I navigate to eBay homepage
    When I select "Toys & Hobbies" from the category dropdown
    And I search for "Monopoly"
    Then I should see "Monopoly" in the first item title
    And shipping to Bulgaria should be available
    And the item price should be displayed
    When I click on the first item
    Then the details page title should contain "Monopoly"
    And the price should match the search page price
    When When I open the shipping details
    Then Bulgaria should be available in the shipping countries
    When I select quantity "2"
    And I click Add to cart
    Then I should be on the cart page
    And the quantity should be "2"
    And the price should be updated for 2 items 