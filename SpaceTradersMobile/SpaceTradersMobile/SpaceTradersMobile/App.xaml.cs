using SpaceTradersMobile.Services;
using SpaceTradersMobile.Views;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpaceTradersMobile
{
   public partial class App : Application
   {

      public App( )
      {
         InitializeComponent( );

         DependencyService.Register< MockDataStore >( );
         DependencyService.Register< SpaceTraderAPI >( );
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
