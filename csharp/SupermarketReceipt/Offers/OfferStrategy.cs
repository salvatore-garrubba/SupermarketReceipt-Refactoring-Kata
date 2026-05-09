using System.Globalization;

namespace SupermarketReceipt.Offers;

public abstract class OfferStrategy
{
    private static readonly CultureInfo Culture = CultureInfo.CreateSpecificCulture("en-GB");
    
    public static string PrintPrice(decimal price)
    {
        return price.ToString("N2", Culture);
    }
    
    public static string PrintPercentage(decimal percent)
    {
        return percent % 1 == 0 
            ? percent.ToString("N0", Culture)
            : percent.ToString("N2", Culture);
    }
}