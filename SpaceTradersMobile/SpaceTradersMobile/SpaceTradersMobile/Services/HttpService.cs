namespace SpaceTradersMobile.Services
{
   using System;
   using System.Net.Http;
   using System.Net.Http.Headers;

   public class HttpService
   {
      private static readonly string baseAddress = "https://api.spacetraders.io/v2/";
      public HttpClient Client;

      public HttpService( )
      {
         this.Client = new HttpClient( )
         {
            BaseAddress = new Uri( baseAddress ),
         };
      }
   }
}
