namespace SpaceTradersMobile.Models
{
   using System.Collections.Generic;

   public class Ship
   {
      public string type { get; set; }
      public string name { get; set; }
      public string description { get; set; }
      public long purchasePrice { get; set; }
      public Frame frame { get; set; }
      public Reactor reactor { get; set; }
      public Engine engine { get; set; }
      public List< Module > modules { get; set; }
      public List< Mount > mounts { get; set; }
   }
}
