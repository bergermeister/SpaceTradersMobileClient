namespace SpaceTradersMobile.Models
{
   public class Module
   {
      public string symbol { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public long capacity { get; set; }
      public Requirements requirements { get; set; }
   }
}
