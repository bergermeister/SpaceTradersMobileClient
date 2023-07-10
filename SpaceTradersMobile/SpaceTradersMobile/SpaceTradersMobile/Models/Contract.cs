namespace SpaceTradersMobile.Models
{
   public class Contract
   {
      public string id { get; set; }
      public string factionSymbol { get; set; }
      public string type { get; set; }
      public Terms terms { get; set; }
      public bool accepted { get; set; }
      public bool fulfilled { get; set; }
      public string expiration { get; set; }
      public string deadlineToAccept { get; set; }
   }
}
