namespace SpaceTradersMobile.Models
{
   using System.Collections.Generic;

   public class Cargo
   {
      public long capacity { get; set; }
      public long units { get; set; }
      public List< object > inventory { get; set; }
   }
}
