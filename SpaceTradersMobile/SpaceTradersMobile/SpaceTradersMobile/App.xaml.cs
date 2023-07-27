namespace SpaceTradersMobile
{
   using SpaceTradersMobile.Services;
   using System;
   using System.Diagnostics;
   using System.IO;
   using System.Net.Http.Headers;
   using Xamarin.Forms;

   public partial class App : Application
   {
      static Data.SystemsDatabase systemeDatabase;

      public static Data.SystemsDatabase SystemDatabase
      {
         get
         {
            if( systemeDatabase == null )
            {
               systemeDatabase = new Data.SystemsDatabase( 
                  Path.Combine( 
                     Environment.GetFolderPath( 
                        Environment.SpecialFolder.LocalApplicationData ), "Systems.db3" ) );
            }
            return systemeDatabase;
         }
      }

      public App( )
      {
         InitializeComponent( );

         DependencyService.Register< MockDataStore >( );
         DependencyService.Register< TokenDataStore >( );
         DependencyService.Register< HttpService >( );
         DependencyService.Register< AgentAPI >( );
         DependencyService.Register< Navigation >( );
         Application.Current.UserAppTheme = OSAppTheme.Dark;
         if( Application.Current.Properties.ContainsKey( "agentToken" ) )
         {
            Services.HttpService httpService = DependencyService.Get< Services.HttpService >( );
            httpService.AuthenticatedClient.DefaultRequestHeaders.Authorization =
               new AuthenticationHeaderValue( "Bearer",
                  Application.Current.Properties[ "agentToken" ] as string );
         }

         MainPage = new AppShell( );
      }

      protected override void OnStart( )
      {
         Debug.WriteLine( "OnStart" );
      }
      protected override void OnSleep( )
      {
         Debug.WriteLine( "OnSleep" );
      }
      protected override void OnResume( )
      {
         Debug.WriteLine( "OnResume" );
      }
   }
}
