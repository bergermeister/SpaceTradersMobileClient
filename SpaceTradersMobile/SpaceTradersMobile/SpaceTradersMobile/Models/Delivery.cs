namespace SpaceTradersMobile.Models
{
   public class Delivery
   {
      public string tradeSymbol { get; set; }
      public string destinationSymbol { get; set; }
      public long unitsRequired { get; set; }
      public long unitsFulfilled { get; set; }
   }
}
