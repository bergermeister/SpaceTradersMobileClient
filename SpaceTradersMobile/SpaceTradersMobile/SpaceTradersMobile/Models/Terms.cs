using System.Collections.Generic;

namespace SpaceTradersMobile.Models
{   public class Terms
   {
      public string deadline { get; set; }
      public Payment payment { get; set; }
      public List< Delivery > deliver { get; set; }
   }
}
