namespace SpaceTradersMobile.Services
{
   using System;
   using System.Net.Http;
   using System.Net.Http.Headers;

   public class HttpService
   {
      private static readonly string baseAddress = "https://api.spacetraders.io/v2/";
      public HttpClient AuthenticatedClient;
      public HttpClient UnauthenticatedClient;

      public HttpService( )
      {
         this.AuthenticatedClient = new HttpClient( )
         {
            BaseAddress = new Uri( baseAddress ),
         };

         this.UnauthenticatedClient = new HttpClient( )
         {
            BaseAddress = new Uri( baseAddress ),
         };
      }
   }
}
