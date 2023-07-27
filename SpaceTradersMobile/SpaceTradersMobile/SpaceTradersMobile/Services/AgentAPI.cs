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
         string token = string.Empty;
         Models.Agent agent = null;
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
               token = jsonObject[ "data" ][ "token" ].ToString( );
               agent = jsonObject[ "data" ][ "agent" ].ToObject<Models.Agent>( );
            }

            this.authClient.DefaultRequestHeaders.Authorization =
               new AuthenticationHeaderValue( "Bearer", token );
            Application.Current.Properties[ "agentToken" ] = token;
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

         return ( new KeyValuePair<string, Models.Agent>( token, agent ) );
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
   
      public async Task< List< Models.Contract > > GetContracts( )
      {
         string requestUri;
         List< Models.Contract > contracts = null;
         Models.Meta meta = new Models.Meta( )
         {
            total = 10,
            limit = 10,
            page = 1
         };
         long page = 1;

         do
         {
            requestUri = String.Format( "my/contracts?limit={0}&page={1}", meta.limit, page );

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
                  contracts = jsonObject[ "data" ].ToObject<List<Models.Contract>>( );
               }

               page++;
               Thread.Sleep( 100 );
            }
            else
            {
               break;
            }
         } while( page < ( meta.total / meta.limit ) );

         return( contracts );
      }

      public async Task< Models.Contract > GetContract( string contractId )
      {
         string requestUri = string.Format( "my/contracts/{0}", contractId );
         Models.Contract contract = null;
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
               contract = jsonObject[ "data" ].ToObject<Models.Contract>( );
            }
         }
         return ( contract );
      }

      public async Task<Models.Contract> AcceptContract( string contractId )
      {
         string requestUri = String.Format( "my/contracts/{0}/accept", contractId );
         Models.Contract contract = null;

         HttpResponseMessage response = await this.authClient.PostAsync( requestUri, null );

         if( response.StatusCode == System.Net.HttpStatusCode.OK )
         {
            // Get the response content.
            HttpContent responseContent = response.Content;

            // Get the stream of the content.
            using( var reader = new StreamReader( await responseContent.ReadAsStreamAsync( ) ) )
            {
               string jsonString = await reader.ReadToEndAsync( );
               JObject jsonObject = JObject.Parse( jsonString );
               contract = jsonObject[ "data" ][ "contract" ].ToObject<Models.Contract>( );
            }
         }

         return ( contract );
      }
   }
}
