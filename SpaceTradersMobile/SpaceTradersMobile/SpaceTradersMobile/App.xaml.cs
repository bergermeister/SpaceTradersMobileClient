namespace SpaceTradersMobile
{
   using SpaceTradersMobile.Services;
   using System.Diagnostics;
   using System.Net.Http.Headers;
   using Xamarin.Forms;

   public partial class App : Application
   {

      public App( )
      {
         InitializeComponent( );

         DependencyService.Register< MockDataStore >( );
         DependencyService.Register< TokenDataStore >( );
         DependencyService.Register< HttpService >( );
         DependencyService.Register< AgentAPI >( );
         DependencyService.Register< ContractAPI >( );
         DependencyService.Register< Navigation >( );

         if( Application.Current.Properties.ContainsKey( "agentToken" ) )
         {
            Services.HttpService httpService = DependencyService.Get< Services.HttpService >( );
            httpService.Client.DefaultRequestHeaders.Authorization =
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
