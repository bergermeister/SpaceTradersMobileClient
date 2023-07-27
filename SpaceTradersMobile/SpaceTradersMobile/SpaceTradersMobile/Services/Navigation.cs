namespace SpaceTradersMobile.Services
{
   using Newtonsoft.Json;
   using Newtonsoft.Json.Linq;
   using System;
   using System.Collections.Generic;
   using System.Diagnostics;
   using System.IO;
   using System.Net.Http;
   using System.Net.Http.Headers;
   using System.Runtime.CompilerServices;
   using System.Text;
   using System.Threading;
   using System.Threading.Tasks;
   using Xamarin.Forms;

   public class Navigation
   {
      private HttpService httpService;
      private HttpClient authClient;
      private HttpClient unauthClient;

      public Navigation( )
      {
         this.httpService = DependencyService.Get< HttpService >( );
         this.authClient = httpService.AuthenticatedClient;
         this.unauthClient = httpService.UnauthenticatedClient;
      }

      public async void DownloadSystems( )
      {
         string requestUri;
         Models.Meta meta = new Models.Meta( )
         {
            total = 5000,
            limit = 20,
            page = 1
         };
         List< Models.System > systems;
         long page = 1;

         do
         {
            // Get the response.
            requestUri = String.Format( "systems?limit={0}&page={1}", meta.limit, page );
            HttpResponseMessage response = await this.unauthClient.GetAsync( requestUri );

            if( response.StatusCode == System.Net.HttpStatusCode.OK )
            {
               // Get the response content.
               HttpContent responseContent = response.Content;

               // Get the stream of the content.
               using( var reader = new StreamReader( await responseContent.ReadAsStreamAsync( ) ) )
               {
                  string jsonString = await reader.ReadToEndAsync( );
                  JObject jsonObject = JObject.Parse( jsonString );
                  meta = jsonObject[ "meta" ].ToObject< Models.Meta >( );

                  systems = jsonObject[ "data" ].ToObject< List< Models.System > >( );
                  foreach( var system in systems )
                  {
                     await App.SystemDatabase.SaveSystemAsync( system );
                  }
               }
               
               page++;
               Thread.Sleep( 100 );
            }
            else
            {
               break;
            }
         } while( page < ( meta.total / meta.limit ) );
      }

      public async Task< Models.Waypoint > GetWaypoint( string systemString, string waypointString )
      {
         string requestUri = String.Format( "systems/{0}/waypoints/{1}", systemString, waypointString );
         Models.Waypoint waypoint = null;

         // Get the response.
         HttpResponseMessage response = await authClient.GetAsync( requestUri );

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
         else
         {
            Debug.WriteLine( string.Format( "{0}:{1}", response.StatusCode, response.ReasonPhrase ) );
         }

         return ( waypoint );
      }

      public async Task< List< Models.Waypoint > > GetSystemWaypoints( string systemString )
      {
         string requestUri;
         Models.Meta meta = new Models.Meta( )
         {
            total = 5000,
            limit = 20,
            page = 1
         };
         List< Models.Waypoint > waypoints = new List< Models.Waypoint >( );
         long page = 1;

         do
         {
            // Get the response.
            requestUri = String.Format( "systems/{0}/waypoints?limit={1}&page={2}", systemString, meta.limit, page );
            HttpResponseMessage response = await this.unauthClient.GetAsync( requestUri );

            if( response.StatusCode == System.Net.HttpStatusCode.OK )
            {
               // Get the response content.
               HttpContent responseContent = response.Content;

               // Get the stream of the content.
               using( var reader = new StreamReader( await responseContent.ReadAsStreamAsync( ) ) )
               {
                  string jsonString = await reader.ReadToEndAsync( );
                  JObject jsonObject = JObject.Parse( jsonString );
                  meta = jsonObject[ "meta" ].ToObject< Models.Meta >( );
                  waypoints.AddRange( jsonObject[ "data" ].ToObject< List< Models.Waypoint > >( ) );
               }

               page++;
               Thread.Sleep( 10 );
            }
            else
            {
               break;
            }
         } while( page < ( meta.total / meta.limit ) );

         return ( waypoints );
      }

      public async Task< List< Models.Ship > > GetAvailableShips( string systemString, string waypointString )
      {
         string requestUri = String.Format( "systems/{0}/waypoints/{1}/shipyard", systemString, waypointString );
         List< Models.Ship > ships = null;

         // Get the response.
         HttpResponseMessage response = await authClient.GetAsync( requestUri );

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
