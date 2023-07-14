using System.Collections.Generic;

namespace SpaceTradersMobile.Models
{
   public class System
   {
      public string symbol { get; set; }
      public string sectorSymbol { get; set; }
      public string type { get; set; }
      public long x { get; set; }
      public long y { get; set; }
      public List< Models.Waypoint > waypoints { get; set; }
      public List< Models.Faction > factions { get; set; }
   }
}
