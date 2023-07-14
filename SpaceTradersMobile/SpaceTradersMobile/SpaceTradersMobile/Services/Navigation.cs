namespace SpaceTradersMobile.Services
{
   using Newtonsoft.Json.Linq;
   using System;
   using System.Collections.Generic;
   using System.IO;
   using System.Net.Http;
   using System.Net.Http.Headers;
   using System.Threading.Tasks;
   using Xamarin.Forms;

   public class Navigation
   {
      private HttpService httpService;
      private HttpClient client;

      public Navigation( )
      {
         this.httpService = DependencyService.Get< HttpService >( );
         this.client = httpService.Client;
      }

      public async Task< List< Models.System > > GetSystems( )
      {
         const string requestUri = "systems";
         List< Models.System > systems = null;

         // Get the response.
         HttpResponseMessage response = await client.GetAsync( requestUri );

         if( response.StatusCode == System.Net.HttpStatusCode.OK )
         {
            // Get the response content.
            HttpContent responseContent = response.Content;

            // Get the stream of the content.
            using( var reader = new StreamReader( await responseContent.ReadAsStreamAsync( ) ) )
            {
               string jsonString = await reader.ReadToEndAsync( );
               JObject jsonObject = JObject.Parse( jsonString );
               systems = jsonObject[ "data" ].ToObject< List< Models.System > >( );
            }
         }

         return ( systems );
      }

      public async Task< Models.Waypoint > GetWaypoint( string systemString, string waypointString )
      {
         string requestUri = String.Format( "systems/{0}/waypoints/{1}", systemString, waypointString );
         Models.Waypoint waypoint = null;

         // Get the response.
         HttpResponseMessage response = await client.GetAsync( requestUri );

         if( response.StatusCode == System.Net.HttpStatusCode.OK )
         {
            // Get the response content.
            HttpContent responseContent = response.Content;

            // Get the stream of the content.
            using( var reader = new StreamReader( await responseContent.ReadAsStreamAsync( ) ) )
            {
               string jsonString = await reader.ReadToEndAsync( );
               JObject jsonObject = JObject.Parse( jsonString );
               waypoint = jsonObject[ "data" ].ToObject< Models.Waypoint >( );
            }
         }

         return ( waypoint );
      }

      public async Task< List< Models.Waypoint > > GetSystemWaypoints( string systemString )
      {
         string requestUri = String.Format( "systems/{0}/waypoints", systemString );
         List< Models.Waypoint > waypoints = null;

         // Get the response.
         HttpResponseMessage response = await client.GetAsync( requestUri );

         if( response.StatusCode == System.Net.HttpStatusCode.OK )
         {
            // Get the response content.
            HttpContent responseContent = response.Content;

            // Get the stream of the content.
            using( var reader = new StreamReader( await responseContent.ReadAsStreamAsync( ) ) )
            {
               string jsonString = await reader.ReadToEndAsync( );
               JObject jsonObject = JObject.Parse( jsonString );
               waypoints = jsonObject[ "data" ].ToObject<List<Models.Waypoint>>( );
            }
         }

         return ( waypoints );
      }

      public async Task< List< Models.Ship > > GetAvailableShips( string systemString, string waypointString )
      {
         string requestUri = String.Format( "systems/{0}/waypoints/{1}/shipyard", systemString, waypointString );
         List< Models.Ship > ships = null;

         // Get the response.
         HttpResponseMessage response = await client.GetAsync( requestUri );

         if( response.StatusCode == System.Net.HttpStatusCode.OK )
         {
            // Get the response content.
            HttpContent responseContent = response.Content;

            // Get the stream of the content.
            using( var reader = new StreamReader( await responseContent.ReadAsStreamAsync( ) ) )
            {
               string jsonString = await reader.ReadToEndAsync( );
               JObject jsonObject = JObject.Parse( jsonString );
               ships = jsonObject[ "data" ][ "ships" ].ToObject< List< Models.Ship > >( );
            }
         }

         return( ships );
      }
   }
}
