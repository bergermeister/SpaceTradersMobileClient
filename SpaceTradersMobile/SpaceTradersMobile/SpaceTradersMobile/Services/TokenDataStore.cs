using SpaceTradersMobile.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceTradersMobile.Services
{
   public class TokenDataStore : IDataStore< KeyValuePair< string, Models.Agent > >
   {
      readonly List< KeyValuePair< string, Models.Agent > > items;

      public TokenDataStore( )
      {
         this.items = new List< KeyValuePair< string, Models.Agent > >( );
      }

      public async Task< bool > AddItemAsync( KeyValuePair< string, Models.Agent > item )
      {
         items.Add( item );

         return( await Task.FromResult( true ) );
      }

      public async Task< bool > UpdateItemAsync( KeyValuePair< string, Models.Agent > item )
      {
         var oldItem = this.items.Where( ( KeyValuePair< string, Models.Agent > arg) => 
            arg.Key == item.Key ).FirstOrDefault( );
         items.Remove( oldItem );
         items.Add( item );

         return( await Task.FromResult( true ) );
      }

      public async Task< bool > DeleteItemAsync( string token )
      {
         var oldItem = this.items.Where( 
            ( KeyValuePair< string, Models.Agent > arg ) => arg.Key == token ).FirstOrDefault( );
         items.Remove( oldItem );

         return( await Task.FromResult( true ) );
      }

      public async Task< KeyValuePair< string, Models.Agent > > GetItemAsync( string token )
      {
         return( await Task.FromResult( items.FirstOrDefault( s => s.Key == token ) ) );
      }

      public async Task< IEnumerable< KeyValuePair< string, Models.Agent > > > GetItemsAsync( 
         bool forceRefresh = false )
      {
         return await Task.FromResult( items );
      }
   }
}
