using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Diagnostics;
using System.Threading;
using System;

namespace SpaceTradersMobile.Services
{
   class AgentAPI
   {
      private HttpService httpService;
      private HttpClient authClient;
      private HttpClient unauthClient;

      public AgentAPI( )
      {
         this.httpService = DependencyService.Get< HttpService >( );
         this.authClient = httpService.AuthenticatedClient;
         this.unauthClient = httpService.UnauthenticatedClient;
      }

      public async Task< KeyValuePair< string, Models.Agent > > Register( string callSign, string requestedFaction )
      {
         const string uri = "register";
         KeyValuePair< string, Models.Agent > agentToken;
         StringContent jsonContent = new StringContent(
            JsonConvert.SerializeObject( new
            {
               symbol = callSign,
               faction = requestedFaction,
            } ),
            Encoding.UTF8,
            "application/json" );

         HttpResponseMessage response = await this.unauthClient.PostAsync( uri, jsonContent );

         if( response.StatusCode == System.Net.HttpStatusCode.Created )
         {
            // Get the response content.
            HttpContent responseContent = response.Content;

            // Get the stream of the content.
            using( var reader = new StreamReader( await responseContent.ReadAsStreamAsync( ) ) )
            {
               string jsonString = await reader.ReadToEndAsync( );
               JObject jsonObject = JObject.Parse( jsonString );
               agentToken = new KeyValuePair< string, Models.Agent >(
                  jsonObject[ "data" ][ "token" ].ToString( ),
                  jsonObject[ "data" ][ "agent" ].ToObject< Models.Agent >( ) );
            }

            this.authClient.DefaultRequestHeaders.Authorization =
               new AuthenticationHeaderValue( "Bearer", agentToken.Key );
            Application.Current.Properties[ "agentToken" ] = agentToken.Key;
            await Application.Current.SavePropertiesAsync( );
         }
         else
         {
            // Get the response content.
            HttpContent responseContent = response.Content;

            // Get the stream of the content.
            using( var reader = new StreamReader( await responseContent.ReadAsStreamAsync( ) ) )
            {
               string jsonString = await reader.ReadToEndAsync( );
               Debug.WriteLine( jsonString );
            }
         }

         return( agentToken );
      }

      public async Task< Models.Agent > GetCurrent( )
      {
         const string requestUri = "my/agent";
         Models.Agent agent = null;

         // Get the response.
         HttpResponseMessage response = await this.authClient.GetAsync( requestUri );

         if( response.StatusCode == System.Net.HttpStatusCode.OK )
         {
            // Get the response content.
            HttpContent responseContent = response.Content;

            // Get the stream of the content.
            using( var reader = new StreamReader( await responseContent.ReadAsStreamAsync( ) ) )
            {
               string jsonString = await reader.ReadToEndAsync( );
               JObject jsonObject = JObject.Parse( jsonString );
               agent = jsonObject[ "data" ].ToObject< Models.Agent >( );
            }
         }

         return( agent );
      }

      public async Task< List< Models.Faction > > GetFactions( )
      {
         string requestUri;
         Models.Meta meta = new Models.Meta( )
         {
            total = 19,
            limit = 10,
            page = 1
         };
         List< Models.Faction > factions = null;
         long page = 1;

         do
         { 
            // Get the response.
            requestUri = String.Format( "factions?limit={0}&page={1}", meta.limit, page );
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
                  meta = jsonObject[ "meta" ].ToObject<Models.Meta>( );

                  factions = jsonObject[ "data" ].ToObject< List< Models.Faction > >( );
               }

               page++;
               Thread.Sleep( 100 );
            }
            else
            {
               break;
            }
         } while( page < ( meta.total / meta.limit ) );

         return( factions );
      }
   }
}
