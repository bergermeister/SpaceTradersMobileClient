namespace SpaceTradersMobile.Services
{
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

   class AgentAPI
   {
      private HttpService httpService;
      private HttpClient client;

      public AgentAPI( )
      {
         this.httpService = DependencyService.Get< HttpService >( );
         this.client = httpService.Client;
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

         HttpResponseMessage response = await this.client.PostAsync( uri, jsonContent );

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
                  jsonObject[ "token" ].ToString( ),
                  jsonObject[ "token" ].ToObject< Models.Agent >( ) );
            }

            this.client.DefaultRequestHeaders.Authorization =
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
         HttpResponseMessage response = await this.client.GetAsync( requestUri );

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
   }
}
