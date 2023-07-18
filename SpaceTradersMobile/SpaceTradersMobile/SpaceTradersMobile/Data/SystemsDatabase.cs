using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SpaceTradersMobile.Data
{
   public class SystemsDatabase
   {
      readonly SQLiteAsyncConnection database;

      public SystemsDatabase( string dbPath )
      {
         this.database = new SQLiteAsyncConnection( dbPath );
         this.database.CreateTableAsync< Models.System >( ).Wait( );
      }

      public Task< List< Models.System > > GetSystemsAsync( )
      {
         return( this.database.Table< Models.System >( ).ToListAsync( ) );
      }

      public Task< List< Models.System > > GetSystemRangeAsync( long x1, long y1, long x2, long y2 )
      {
         return( database.Table< Models.System >( )
            .Where( s => ( ( s.x <= x1 ) && ( s.x >= x2 ) && ( s.y <= y1 ) && ( s.y >= y2 ) ) )
            .ToListAsync( ) );
      }

      public Task< Models.System > GetSystemAsync( string symbol )
      {
         return( database.Table< Models.System >( )
            .Where( s => s.symbol == symbol )
            .FirstOrDefaultAsync( ) );
      }

      public Task< int > SaveSystemAsync( Models.System system )
      {
         return( database.InsertOrReplaceAsync( system ) );
      }

      public Task< int > DeleteSystemAsync( Models.System system )
      {
         return( this.database.DeleteAsync( system ) );
      }
   }
}
