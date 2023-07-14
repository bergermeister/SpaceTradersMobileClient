using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SpaceTradersMobile.Services
{
   public class ContractAPI
   {
      private HttpService httpService;
      private HttpClient client;

      public ContractAPI( )
      {
         this.httpService = DependencyService.Get< HttpService >( );
         this.client = httpService.Client;
      }

      public async Task< List< Models.Contract > > GetCurrent( )
      {
         const string requestUri = "my/contracts";
         List< Models.Contract > contracts = null;

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
               contracts = jsonObject[ "data" ].ToObject<List<Models.Contract>>( );
            }
         }

         return ( contracts );
      }

      public async Task< Models.Contract > AcceptContract( string contractId )
      {
         string requestUri = String.Format( "my/contracts/{0}/accept", contractId );
         Models.Contract contract = null;

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
               contract = jsonObject[ "data" ][ "contract" ].ToObject<Models.Contract>( );
            }
         }

         return ( contract );
      }
   }
}
