using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpaceTradersMobile.Views
{
   [XamlCompilation( XamlCompilationOptions.Compile )]
   public partial class NavigationPage : ContentPage
   {
      private Services.AgentAPI agentAPI;
      private Services.Navigation navigation;
      private ViewModels.UniverseViewModel universeVM;

      public NavigationPage( )
      {
         InitializeComponent( );

         this.agentAPI = DependencyService.Get< Services.AgentAPI >( );
         this.navigation = DependencyService.Get< Services.Navigation >( );
         this.universeVM = new ViewModels.UniverseViewModel( );
         this.UniverseCoordinateEntry.BindingContext = this.universeVM;
      }

      protected override void OnAppearing( )
      {
         this.universeVM.ReloadSystems( );
      }

      protected override void OnDisappearing( )
      {
         base.OnDisappearing( );
      }

      private void CanvasViewPaintSurface( object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs eventArgs )
      {
         //SKImageInfo info = eventArgs.Info;
         SKSurface surface = eventArgs.Surface;
         SKCanvas canvas = surface.Canvas;
         this.universeVM.Draw( canvas );      
      }

      private void CanvasViewTouch( object sender, SKTouchEventArgs args )
      {
         this.universeVM.Touch( sender as SKCanvasView, args );
      }

      private void ViewCoordinatesClicked( object sender, EventArgs e )
      {
         this.universeCoordinatesChanged( );
      }

      private void HeadquartersClicked( object sender, EventArgs e )
      {
         if( Application.Current.Properties.ContainsKey( "agentToken" ) )
         {
            findAgentHeadquarters( );
         }
      }

      private void DownloadSystemsClicked( object sender, EventArgs e )
      {
         navigation.DownloadSystems( );
      }

      private async void universeCoordinatesChanged( )
      {
         await this.universeVM.ReloadSystems( );
         this.CanvasView.InvalidateSurface( );
      }

      private async void findAgentHeadquarters( )
      {
         Models.Agent agent = await agentAPI.GetCurrent( );
         long maxCoord = 0;
         if( agent == null )
         {

         }
         else
         { 
            string[ ] parsedWaypoint = agent.headquarters.Split( new char[ ]{ '-' } );
            string systemSymbol = parsedWaypoint[ 0 ] + '-' + parsedWaypoint[ 1 ];
            // Models.System system = await App.SystemDatabase.GetSystemAsync( systemSymbol );
            List< Models.Waypoint > systemWaypoints = await navigation.GetSystemWaypoints( systemSymbol );
            if( systemWaypoints != null )
            {
               foreach( var waypoint in systemWaypoints )
               {
                  if( Math.Abs( waypoint.x ) > maxCoord ) maxCoord = Math.Abs( waypoint.x );
                  else if( Math.Abs( waypoint.y ) > maxCoord ) maxCoord = Math.Abs( waypoint.y );
                  // Find min x,y and max x,y
                  // Calculate width and height
                  // During draw routine, scale the distances accordingly
               }

               //systemWidth = maxCoord * 3;
               //systemHeight = maxCoord * 3; 

               this.Dispatcher.BeginInvokeOnMainThread( ( ) =>
               {
                  //this.waypoints = systemWaypoints;
                  this.CanvasView.InvalidateSurface( );
               } );
            }
         }
      }
   }
}