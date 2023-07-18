using System.Collections.Generic;
using SQLite;

namespace SpaceTradersMobile.Models
{
   public class System
   {
      [PrimaryKey]
      public string symbol { get; set; }
      public string sectorSymbol { get; set; }
      public string type { get; set; }
      public long x { get; set; }
      public long y { get; set; }
      [Ignore]
      public List< Waypoint > waypoints { get; set; }
      [Ignore]
      public List< Faction > factions { get; set; }
   }
}
