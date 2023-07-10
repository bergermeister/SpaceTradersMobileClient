namespace SpaceTradersMobile.Models
{
   public class Frame
   {
      public string symbol { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public long moduleSlots { get; set; }
      public long mountingPoints { get; set; }
      public long fuelCapacity { get; set; }
      public Requirements requirements { get; set; }
   }
}
