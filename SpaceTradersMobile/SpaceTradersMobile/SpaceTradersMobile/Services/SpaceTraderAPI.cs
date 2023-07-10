namespace SpaceTradersMobile.Services
{
   using Newtonsoft.Json;
   using Newtonsoft.Json.Linq;
   using SpaceTradersMobile.Models;
   using System;
   using System.Collections.Generic;
   using System.IO;
   using System.Net.Http;
   using System.Net.Http.Headers;
   using System.Runtime.CompilerServices;
   using System.Text;
   using System.Threading.Tasks;

   public class SpaceTraderAPI
   {
      private static readonly string baseAddress = "https://api.spacetraders.io/v2/";
      private static HttpClient client = new HttpClient( )
      {
         BaseAddress = new Uri( baseAddress ),
      };
      private const string token = "ENTER DEBUG TOKEN";

      public async Task< bool > RegisterAgent( string callSign, string requestedFaction )
      {
         const string uri = "register";
         StringContent jsonContent = new StringContent(
            JsonConvert.SerializeObject( new
            {
               symbol = callSign,
               faction = requestedFaction,
            } ),
            Encoding.UTF8,
            "application/json" );

         HttpResponseMessage response = await client.PostAsync( uri, jsonContent );

         response.EnsureSuccessStatusCode( );

         var jsonResponse = await response.Content.ReadAsStringAsync();
         Console.WriteLine( $"{jsonResponse}\n" );

         return ( await Task.FromResult( true ) );
      }

      public async Task< Models.Agent > GetAgent( )
      {
         const string requestUri = "my/agent";
         Models.Agent agent = null;
         client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue( "Bearer", token );

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
               agent = jsonObject[ "data" ].ToObject<Models.Agent>( );
            }
         }

         return( agent );
      }

      public async Task< Models.Waypoint > GetWaypoint( string systemString, string waypointString )
      {
         string requestUri = String.Format( "systems/{0}/waypoints/{1}", systemString, waypointString );
         Models.Waypoint waypoint = null;
         client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue( "Bearer", token );

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

         return( waypoint );
      }

      public async Task< List< Models.Waypoint > > GetSystemWaypoints( string systemString )
      {
         string requestUri = String.Format( "systems/{0}/waypoints", systemString );
         List< Models.Waypoint > waypoints = null;
         client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue( "Bearer", token );

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
               waypoints = jsonObject[ "data" ].ToObject< List< Models.Waypoint> >( );
            }
         }

         return( waypoints );
      }

      public async Task< List< Models.Contract > > GetContracts( )
      {
         const string requestUri = "my/contracts";
         List< Models.Contract > contracts = null;
         client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue( "Bearer", token );

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
               contracts = jsonObject[ "data" ].ToObject< List< Models.Contract > >( );
            }
         }

         return( contracts );
      }

      public async Task< Models.Contract > AcceptContract( string contractId )
      {
         string requestUri = String.Format( "my/contracts/{0}/accept", contractId );
         Models.Contract contract = null;
         client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue( "Bearer", token );
         HttpResponseMessage response = await client.PostAsync( requestUri, null );

         if( response.StatusCode == System.Net.HttpStatusCode.OK )
         {
            // Get the response content.
            HttpContent responseContent = response.Content;

            // Get the stream of the content.
            using( var reader = new StreamReader( await responseContent.ReadAsStreamAsync( ) ) )
            {
               string jsonString = await reader.ReadToEndAsync( );
               JObject jsonObject = JObject.Parse( jsonString );
               contract = jsonObject[ "data" ][ "contract" ].ToObject< Models.Contract >( );
            }
         }

         return( contract );
      }

      public async Task< List< Models.Ship > > GetAvailableShips( string systemString, string waypointString )
      {
         string requestUri = String.Format( "systems/{0}/waypoints/{1}/shipyard", systemString, waypointString );
         List< Models.Ship > ships = null;
         client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue( "Bearer", token );

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
