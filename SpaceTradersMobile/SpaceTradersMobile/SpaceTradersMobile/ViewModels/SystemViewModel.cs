using System.Collections.Generic;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace SpaceTradersMobile.ViewModels
{
   public class SystemViewModel : BaseViewModel
   {
      private Models.System system;
      private Services.Navigation navigation;

      public SystemViewModel( )
      {
         this.Title = "Unknown System";
         this.system = null;
         this.navigation = DependencyService.Get< Services.Navigation >( );
      }

      public void CanvasViewPaintSurface( object sender, SKPaintSurfaceEventArgs eventArgs )
      {
         SKImageInfo info = eventArgs.Info;
         SKSurface surface = eventArgs.Surface;
         SKCanvas canvas = surface.Canvas;
         Dictionary< SKPoint, long > overlap = new Dictionary< SKPoint, long >( );
         SKPoint point = new SKPoint( );
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
         for( var i = 1; i < info.Width; i += ( info.Width / 10 ) )
         {
            canvas.DrawCircle( info.Width / 2, info.Height / 2, i, orbitPaint );
         }

         if( this.system == null )
         {
            /// @todo
         }
         else if( this.system.waypoints == null )
         {
            /// @todo
         }
         else
         {
            // Draw waypoints
            if( this.system.waypoints != null )
            {
               foreach( var waypoint in this.system.waypoints )
               {
                  /*
                  point.X = ( ( waypoint.x + ( systemWidth / 2 ) ) * info.Width ) / systemWidth;
                  point.Y = ( ( waypoint.y + ( systemHeight / 2 ) ) * info.Height ) / systemHeight;
                  if( !overlap.ContainsKey( point ) )
                  {
                     overlap.Add( point, 0 );
                  }
                  overlap[ point ]++;

                  switch( waypoint.type )
                  {
                     case "PLANET":
                     {
                        break;
                     }
                     case "JUMP_GATE":
                     {
                        break;
                     }
                     case "GAS_GIANT":
                     {
                        break;
                     }
                     case "ORBITAL_STATION":
                     {
                        break;
                     }
                     case "ASTEROID_FIELD":
                     {
                        break;
                     }
                     case "MOON":
                     {
                        break;
                     }
                     default:
                        break;
                  }
                  canvas.DrawCircle( point, 10, waypointPaint );

                  canvas.DrawText(
                     waypoint.type,
                     point.X - 15,
                     point.Y + ( 20 * overlap[ point ] ), textPaint );
                  */
               }
            }
         }

         canvas.Flush( );
      }   
   }
}
