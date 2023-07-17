using Newtonsoft.Json;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpaceTradersMobile.Views
{
   [XamlCompilation( XamlCompilationOptions.Compile )]
   public partial class NavigationPage : ContentPage
   {
      private Services.AgentAPI agentAPI = DependencyService.Get< Services.AgentAPI >( );
      private Services.Navigation navigation = DependencyService.Get< Services.Navigation >( );
      private List< Models.Waypoint > waypoints = null;
      private long systemWidth;
      private long systemHeight;

      private long x { get; set; }
      private long y { get; set; }

      public NavigationPage( )
      {
         InitializeComponent( );
      }

      protected override void OnAppearing( )
      {
         if( Application.Current.Properties.ContainsKey( "agentToken" ) )
         {
            findAgentHeadquarters( );
         }
      }

      protected override void OnDisappearing( )
      {
         base.OnDisappearing( );
      }

      private void CanvasViewPaintSurface( object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs eventArgs )
      {
         SKImageInfo info = eventArgs.Info;
         SKSurface surface = eventArgs.Surface;
         SKCanvas canvas = surface.Canvas;

         canvas.Clear( );

         // Define colors
         SKPaint gridPaint = new SKPaint
         {
            Style = SKPaintStyle.Stroke,
            Color = Color.White.ToSKColor( ),
            StrokeWidth = 1
         };
         SKPaint waypointPaint = new SKPaint
         {
            Style = SKPaintStyle.Stroke,
            Color = Color.Red.ToSKColor( ),
            StrokeWidth = 1
         };

         // Draw the Grid
         for( var i = 0; i < info.Width; i += ( info.Width / 10 ) )
         {
            canvas.DrawLine( i, 0, i, info.Height, gridPaint );
         }

         for( var i = 0; i < info.Height; i += ( info.Width / 10 ) )
         {
            canvas.DrawLine( 0, i, info.Width, i, gridPaint );
         }

         // Draw waypoints
         if( this.waypoints != null )
         {
            foreach( var waypoint in this.waypoints )
            {
               canvas.DrawCircle( waypoint.x + 30, waypoint.y + 30, 3, waypointPaint );
            }
         }

         canvas.Flush( );
      }

      private void DownloadSystemsClicked( object sender, EventArgs e )
      {
         navigation.DownloadSystems( );
      }

      private async void findAgentHeadquarters( )
      {
         Models.Agent agent = await agentAPI.GetCurrent( );
         string[ ] parsedWaypoint = agent.headquarters.Split( new char[ ]{ '-' } );
         string systemSymbol = parsedWaypoint[ 0 ] + '-' + parsedWaypoint[ 1 ];
         // Models.System system = await App.SystemDatabase.GetSystemAsync( systemSymbol );
         List< Models.Waypoint > systemWaypoints = await navigation.GetSystemWaypoints( systemSymbol );
         if( systemWaypoints != null )
         {
            foreach( var waypoint in systemWaypoints )
            {
               // Find min x,y and max x,y
               // Calculate width and height
               // During draw routine, scale the distances accordingly
            }

            this.Dispatcher.BeginInvokeOnMainThread( ( ) =>
            {
               this.waypoints = systemWaypoints;
               this.CanvasView.InvalidateSurface( );
            } );
         }
      }
   }
}