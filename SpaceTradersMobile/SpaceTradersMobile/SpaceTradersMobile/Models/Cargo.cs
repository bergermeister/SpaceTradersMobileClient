using System.Collections.Generic;

namespace SpaceTradersMobile.Models
{
   public class Cargo
   {
      public long capacity { get; set; }
      public long units { get; set; }
      public List< object > inventory { get; set; }
   }
}
