namespace SpaceTradersMobile.Models
{
   using System.Collections.Generic;

   public class Terms
   {
      public string deadline { get; set; }
      public Payment payment { get; set; }
      public List< Delivery > deliver { get; set; }
   }
}
