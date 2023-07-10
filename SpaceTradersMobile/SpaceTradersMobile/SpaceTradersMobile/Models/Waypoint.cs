namespace SpaceTradersMobile.Models
{
   using System.Collections.Generic;

   /**
    * Every waypoint has a symbol such as 'X1-DF55-20250Z' made up of the sector, system, and location of the 
    * waypoint. For example, 'X1' is the sector, 'X1-DF55
    */
   public class Waypoint
   {
      public string systemSymbol { get; set; }
      public string symbol { get; set; }
      public string type { get; set; }
      public long x { get; set; }
      public long y { get; set; }
      public List< Orbital > orbitals;
      public List< Trait > traits;
      public Chart chart;
      public Faction faction;
   
   }
}
