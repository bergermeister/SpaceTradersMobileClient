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
         SKPaint orbitPaint = new SKPaint
         {
            Style = SKPaintStyle.Stroke,
            Color = Color.Blue.ToSKColor( ),
            StrokeWidth = 1
         };
         SKPaint waypointPaint = new SKPaint
         {
            Style = SKPaintStyle.Stroke,
            Color = Color.Red.ToSKColor( ),
            StrokeWidth = 3
         };
         SKPaint textPaint = new SKPaint
         {
            Style = SKPaintStyle.Stroke,
            Color = Color.Yellow.ToSKColor( ),
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
         for( var i = 1; i < info.Width; i += ( info.Width / 10 ) )
         {
            canvas.DrawCircle( info.Width / 2, info.Height / 2, i, orbitPaint );
         }

         // Draw waypoints
         if( this.waypoints != null )
         {
            foreach( var waypoint in this.waypoints )
            {
               long centerX = ( ( waypoint.x + ( systemWidth / 2 ) ) * info.Width ) / systemWidth;
               long centerY = ( ( waypoint.y + ( systemHeight / 2 ) ) * info.Height ) / systemHeight;
               canvas.DrawCircle( centerX, centerY, 10, waypointPaint );
               canvas.DrawText( waypoint.type, centerX - 15, centerY + 15, textPaint );
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

               systemWidth = maxCoord * 3;
               systemHeight = maxCoord * 3; 

               this.Dispatcher.BeginInvokeOnMainThread( ( ) =>
               {
                  this.waypoints = systemWaypoints;
                  this.CanvasView.InvalidateSurface( );
               } );
            }
         }
      }
   }
}