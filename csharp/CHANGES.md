# Changes and reasoning behind them

## Fist impressions

Code is not very clean, it is a bit hard to understand at first, but it is not too difficult to figure out what it does. 

It looks like the code has clear refactoring opportunities, especially in discount logic.
In fact, the `ShoppingCart` class is handling the logic of calculating the total price, but it is also handling the logic of applying discounts, which should be a separate concern.
The logic there is also a bit convoluted, with a lot of if statements and nested loops, which makes it hard to understand and maintain (violating the Single Responsibility Principle).
There's only one test in both NUnit and XUnit projects, and it is not testing the discount logic at all.

Furthermore, running the tests fails with the following error:
```
A total of 1 test files matched the specified pattern.
Failed! - Failed: 0, Passed: 0, Skipped: 0, Total: 0, Duration: 98ms

An exception occurred while invoking executor 'executor://xunit/VsTestRunner2/netcoreapp': Could not load file or assembly 'xunit.abstractions, Version=2.0.0.0, Culture=neutral, PublicKeyToken=8d05b1bb7a6fdb6c'. 
The system cannot find the file specified.
```
From a quick search, it seems that this is a common issue with xUnit.
I changed the SDK from MSTest.Sdk to Microsoft.NET.Sdk, which was fighting with the xUnit dependencies and causing the runtime issue.

Similar issue with the NUnit test project, which was also using `xunit.runner.visualstudio` (unnecessary for NUnit) and added the test adapter to enable proper discover.

## Added unit tests to cover simple scenarios
I'm more familiar with XUnit, so I decided to use this project and not NUnit.
I've added a few unit tests to cover the discounts, based on obsarvable behaviour rather than internal logic. I've noticed that a few tests were initially failing, because of the use of `double` instead of `decimal`. In short, it's way better to use `decimal` because it uses base-10 floating point instead of binary (which cannot represent most decimal fractions exactly), but for now I decided to keep it like this and refactor later on. For now, a quick `Math.Round` with 2 fractional digits should be enough.

## Changed from double to decimal
I decided to refactor the code to use `decimal` and round to 2 decimal places. This ensures that all prices, total and discounts are handles with cent precision, preventing floating-point issues in monetary calculations. 

## Initial implementation of the strategy pattern
The `ShoppingCart` class is handling the logic to calculate discount in a wrong way, it should not be its responsibility. I created an interface called `IOfferStrategy` to make a contract for calculating the discount and an abstract class `OfferStrategy` in order to us shared methods like `PrintPrice()`. This is the first part of the refactoring for the strategy pattern, the logic is still in the wrong place. I also moved all the classes in a separate file, to make them visible at the solution level.

## Final implementation of the strategy pattern
The `Offer` class is not in charge of the logic to intialize the `IOfferStrategy` and exposes a public method `CalculateDiscount()` that calculate the `Discount`. This logic has been removed from the `ShoppingCart`, which simply call the `offer.CalculateDiscount()` method. 
I've added basic validation to the classes handling the logic for the offer, like negative values and null parameters. 
I've also added unit tests for the classed that implement the `IOfferStrategy` interface, still missing a few but will add later on.

## Added remaining unit tests for strategy
I've added all the remaining unit tests for the missing classes that implement the `IOfferStrategy` interface. 
I've also added very basic argument checking for all the other classes.
Furthermore, I've made the `Product` name case invariant, adding proper unit tests to cover this scenario.

## Better handling of null values
My last step is to handle in a better way the `null` values.
I've added a `DiscountResult` discriminated union using a readonly struct to better handling the result of `Calculate` method in the `IOfferStrategy` interface.
This struct has basically 2 values: `None` if no discount was found, and `Value` if the discount was correctly found.
