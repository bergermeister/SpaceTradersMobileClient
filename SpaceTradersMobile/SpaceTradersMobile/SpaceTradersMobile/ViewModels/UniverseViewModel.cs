using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace SpaceTradersMobile.ViewModels
{
   public class UniverseViewModel : BaseViewModel
   {
      public readonly long MaxRadius = 65000;
      private List< Models.System > systems;
      private List< Models.System > visibleSystems;
      private Dictionary< string, SKPaint > paint;
      private Dictionary< string, long > size;
      private double x;
      private double y;
      private double radius;
      
      public UniverseViewModel( )
      {
         this.ReloadSystems( );
         this.visibleSystems = new List<Models.System>( );
         this.Title = "Universe";
         this.x = 0;
         this.y = 0;
         this.radius = 100;
         this.paint = new Dictionary<string, SKPaint>
         {
            { "BLUE_STAR",    new SKPaint { Style = SKPaintStyle.StrokeAndFill, Color = Color.DarkBlue.ToSKColor( ), StrokeWidth = 1 } },
            { "YOUNG_STAR",   new SKPaint { Style = SKPaintStyle.StrokeAndFill, Color = Color.LightYellow.ToSKColor( ), StrokeWidth = 1 } },
            { "ORANGE_STAR",  new SKPaint { Style = SKPaintStyle.StrokeAndFill, Color = Color.Orange.ToSKColor( ), StrokeWidth = 1 } },
            { "RED_STAR",     new SKPaint { Style = SKPaintStyle.StrokeAndFill, Color = Color.Red.ToSKColor( ), StrokeWidth = 1 } },
            { "BLACK_HOLE",   new SKPaint { Style = SKPaintStyle.StrokeAndFill, Color = Color.White.ToSKColor( ), StrokeWidth = 1 } },
            { "NEUTRON_STAR", new SKPaint { Style = SKPaintStyle.StrokeAndFill, Color = Color.LightBlue.ToSKColor( ), StrokeWidth = 1 } },
            { "WHITE_DWARF",  new SKPaint { Style = SKPaintStyle.StrokeAndFill, Color = Color.LightCyan.ToSKColor( ), StrokeWidth = 1 } },
            { "HYPERGIANT",   new SKPaint { Style = SKPaintStyle.StrokeAndFill, Color = Color.Blue.ToSKColor( ), StrokeWidth = 1 } },
            { "UNSTABLE",     new SKPaint { Style = SKPaintStyle.StrokeAndFill, Color = Color.Purple.ToSKColor( ), StrokeWidth = 1 } }
         };
         this.size = new Dictionary<string, long>
         {
            { "BLUE_STAR",    10 },
            { "YOUNG_STAR",   10 },
            { "ORANGE_STAR",  10 },
            { "RED_STAR",     10 },
            { "BLACK_HOLE",   10 },
            { "NEUTRON_STAR", 10 },
            { "WHITE_DWARF",  5 },
            { "HYPERGIANT",   20 },
            { "UNSTABLE",     10 }
         };
      }

      public double X
      { 
         get => this.x; 
         set
         {
            SetProperty( ref this.x, value );
            OnPropertyChanged( "X" );
         }
      }
      public double Y 
      { 
         get => this.y; 
         set
         {
            SetProperty( ref this.y, value );
            OnPropertyChanged( "Y" );
         }
      }
      public double Radius 
      { 
         get => this.radius; 
         set
         {
            SetProperty( ref this.radius, value );
            OnPropertyChanged( "Radius" );
         }
      }

      public List< Models.System > VisibleSystems { get => this.visibleSystems; }

      public async Task< List< Models.System > > ReloadSystems( )
      {
         //long x1 = ( long )( this.x - this.radius );
         //long y1 = ( long )( this.y - this.radius );
         //long x2 = ( long )( this.x + this.radius );
         //long y2 = ( long )( this.y + this.radius );
         //this.systems = await App.SystemDatabase.GetSystemRangeAsync( x1, y1, x2, y2 );
         this.systems = await App.SystemDatabase.GetSystemsAsync( );
         return( this.systems );
      }

      public void Draw( SKImageInfo info, SKSurface skSurface, SKCanvas canvas )
      {
         SKRect clipBox;
         //clipBox = canvas.LocalClipBounds;
         clipBox = new SKRect( 0, 0, ( float )canvas.LocalClipBounds.Width, ( float )canvas.LocalClipBounds.Height );
         clipBox = canvas.TotalMatrix.MapRect( clipBox );

         // Update X and Y coordinates of center
         this.X = clipBox.MidX;
         this.Y = clipBox.MidY;
         this.visibleSystems.Clear( );

         // Define colors
         SKPaint gridPaint = new SKPaint
         {
            Style = SKPaintStyle.Stroke,
            Color = Color.Gray.ToSKColor( ),
            StrokeWidth = 1
         };
         SKPaint textPaint = new SKPaint
         {
            Style = SKPaintStyle.Stroke,
            Color = Color.Yellow.ToSKColor( ),
            StrokeWidth = 1
         };

         // Draw the Grid
         for( var i = ( ( long )canvas.LocalClipBounds.Left / 50 ) * 50; i < canvas.LocalClipBounds.Right; i += 50 )
         {
            canvas.DrawLine( i, clipBox.Top, i, clipBox.Bottom, gridPaint );
         }

         for( var i = ( ( long )canvas.LocalClipBounds.Top / 50 ) * 50; i < canvas.LocalClipBounds.Right; i += 50 )
         {
            canvas.DrawLine( clipBox.Left, i, clipBox.Right, i, gridPaint );
         }

         // Draw Systems
         if( this.systems == null )
         {
            /// @todo
         }
         else 
         {
            // Determine if the touch was within that rectangle
            foreach( var system in this.systems )
            {
               if( clipBox.Contains( system.x, system.y ) )
               {
                  this.visibleSystems.Add( system );
                  canvas.DrawCircle( system.x, system.y, 10, paint[ system.type ] );
                  canvas.DrawText( system.symbol, system.x - 15, system.y + 20, textPaint );
                  canvas.DrawText( system.type, system.x - 15, system.y + 40, textPaint );
               }
            }
         }

         canvas.Flush( );
      }
   }
}
