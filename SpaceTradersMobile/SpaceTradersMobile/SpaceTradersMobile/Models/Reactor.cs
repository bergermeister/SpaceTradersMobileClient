namespace SpaceTradersMobile.Models
{
   public class Reactor
   {
      public string symbol { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public long powerOutput { get; set; }
      public Requirements requirements { get; set; }
   }
}
