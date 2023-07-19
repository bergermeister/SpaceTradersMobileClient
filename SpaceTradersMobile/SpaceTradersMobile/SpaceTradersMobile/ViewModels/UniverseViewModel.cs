using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using SpaceTradersMobile.Models;
using SpaceTradersMobile.Views;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace SpaceTradersMobile.ViewModels
{
   public class UniverseViewModel : BaseViewModel
   {
      private List< Models.System > systems;
      private List< Models.System > visibleSystems;
      private double x;
      private double y;
      private SKMatrix matrix;
      private Dictionary< long, SKPoint > touchDictionary;
      private Dictionary< string, SKPaint > paint;
      private Dictionary< string, long > size;

      public UniverseViewModel( )
      {
         this.ReloadSystems( );
         this.visibleSystems = new List<Models.System>( );
         this.Title = "Universe";
         this.x = 0;
         this.y = 0;
         this.matrix = SKMatrix.CreateIdentity( );
         this.touchDictionary = new Dictionary<long, SKPoint>( );
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

      public List< Models.System > VisibleSystems { get => this.visibleSystems; }

      public async Task< List< Models.System > > ReloadSystems( )
      {
         this.systems = await App.SystemDatabase.GetSystemsAsync( );
         return( this.systems );
      }

      public void Draw( SKCanvas canvas )
      {
         canvas.Clear( );
         canvas.SetMatrix( matrix );

         SKRect clipBox;
         clipBox = new SKRect( 0, 0, ( float )canvas.LocalClipBounds.Width, ( float )canvas.LocalClipBounds.Height );
         clipBox = canvas.TotalMatrix.Invert( ).MapRect( clipBox );

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
         for( var i = ( ( long )clipBox.Left / 50 ) * 50; i < clipBox.Right; i += 50 )
         {
            canvas.DrawLine( i, clipBox.Top, i, clipBox.Bottom, gridPaint );
         }

         for( var i = ( ( long )clipBox.Top / 50 ) * 50; i < clipBox.Bottom; i += 50 )
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
                  canvas.DrawCircle( system.x, system.y, size[ system.type ], paint[ system.type ] );
                  canvas.DrawText( system.symbol, system.x - 15, system.y + 20, textPaint );
                  canvas.DrawText( string.Format( "({0},{1})", system.x, system.y ), system.x - 15, system.y + 40, textPaint );
                  //canvas.DrawText( system.type, system.x - 15, system.y + 40, textPaint );
               }
            }
         }

         canvas.Flush( );
      }
   
      public void Touch( SKCanvasView canvasView, SKTouchEventArgs args )
      {
         SKPoint point = args.Location;

         switch( args.ActionType )
         {
            case SKTouchAction.Pressed:
            {
               // Find transformed canvasview rectangle
               SKRect rect = new SKRect( point.X - 10, point.Y - 10, point.X + 10, point.Y + 10 );
               rect = matrix.Invert( ).MapRect( rect );

               bool systemClicked = false;
               foreach( var system in this.VisibleSystems )
               {
                  if( rect.Contains( system.x, system.y ) )
                  {
                     Debug.WriteLine( string.Format( "Click system: {0}", system.symbol ) );
                     systemClicked = true;
                     OnSystemSelected( system );
                     break;
                  }
               }

               // Determine if the touch was within that rectangle
               if( !systemClicked && !touchDictionary.ContainsKey( args.Id ) )
               {
                  touchDictionary.Add( args.Id, point );
               }
               break;
            }
            case SKTouchAction.Moved:
            {
               if( touchDictionary.ContainsKey( args.Id ) )
               {
                  if( touchDictionary.Count == 1 )
                  {
                     SKPoint prevPoint = touchDictionary[ args.Id ];
                     touchDictionary[ args.Id ] = point;
                     matrix.TransX += point.X - prevPoint.X;
                     matrix.TransY += point.Y - prevPoint.Y;
                     canvasView.InvalidateSurface( );
                  }
                  else if( touchDictionary.Count >= 2 )
                  {
                     // Copy two dictionary keys into array
                     long[ ] keys = new long[ touchDictionary.Count ];
                     touchDictionary.Keys.CopyTo( keys, 0 );

                     // Find index of non-moving (pivot) finger
                     int pivotIndex = ( keys[ 0 ] == args.Id ) ? 1 : 0;

                     // Get the three points involved in the transform
                     SKPoint pivotPoint = touchDictionary[ keys[ pivotIndex ] ];
                     SKPoint prevPoint = touchDictionary[ args.Id ];
                     SKPoint newPoint = point;

                     // Calculate two vectors
                     SKPoint oldVector = prevPoint - pivotPoint;
                     SKPoint newVector = newPoint - pivotPoint;

                     // Scaling factors are ratios of the vectors
                     float scaleX = newVector.X / oldVector.X;
                     float scaleY = newVector.Y / oldVector.Y;

                     if( !float.IsNaN( scaleX ) && !float.IsInfinity( scaleX ) &&
                         !float.IsNaN( scaleY ) && !float.IsInfinity( scaleY ) )
                     {
                        // If something bad hasn't happened, calculate a scale and translation matrix
                        SKMatrix scaleMatrix = SKMatrix.CreateScale( scaleX, scaleY, pivotPoint.X, pivotPoint.Y );
                        matrix.PostConcat( scaleMatrix );
                        canvasView.InvalidateSurface( );
                     }
                  }
               }
               break;
            }
            case SKTouchAction.Released: // Fall through
            case SKTouchAction.Cancelled:
            {
               touchDictionary.Remove( args.Id );
               break;
            }
         }
      }

      public async void OnSystemSelected( Models.System system )
      {
         if( system == null )
         {
         }
         else
         {
            // This will push the SystemPage onto the navigation stack
            await Shell.Current.GoToAsync( $"{nameof( SystemPage )}?{nameof( SystemViewModel.Symbol )}={system.symbol}" );
         }
      }
   }
}
