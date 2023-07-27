using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using SpaceTradersMobile.Models;
using Xamarin.Forms;

namespace SpaceTradersMobile.ViewModels
{
   [QueryProperty( nameof( Symbol ), nameof( Symbol ) )]
   public class SystemViewModel : BaseViewModel
   {
      private string symbol;
      private Models.System system;
      private Services.Navigation navigation;
      private ObservableCollection< WaypointViewModel > waypoints;
      private SKMatrix matrix;
      private Dictionary< long, SKPoint > touchDictionary;
      private Dictionary< string, SKPaint > paint;
      private Dictionary< string, long > size;
      private bool listView;

      public SystemViewModel( )
      {
         this.Title = "Unknown System";
         this.system = new Models.System( )
         {
            symbol = "Unknown"
         };
         this.navigation = DependencyService.Get< Services.Navigation >( );
         this.waypoints = new ObservableCollection< WaypointViewModel >( );
         this.matrix = SKMatrix.CreateIdentity( );
         this.touchDictionary = new Dictionary<long, SKPoint>( );
         this.paint = new Dictionary<string, SKPaint>
         {
            { "PLANET",          new SKPaint { Style = SKPaintStyle.StrokeAndFill, Color = Color.Green.ToSKColor( ), StrokeWidth = 1 } },
            { "JUMP_GATE",       new SKPaint { Style = SKPaintStyle.StrokeAndFill, Color = Color.Purple.ToSKColor( ), StrokeWidth = 1 } },
            { "GAS_GIANT",       new SKPaint { Style = SKPaintStyle.StrokeAndFill, Color = Color.Brown.ToSKColor( ), StrokeWidth = 1 } },
            { "ORBITAL_STATION", new SKPaint { Style = SKPaintStyle.StrokeAndFill, Color = Color.White.ToSKColor( ), StrokeWidth = 1 } },
            { "ASTEROID_FIELD",  new SKPaint { Style = SKPaintStyle.StrokeAndFill, Color = Color.DarkGray.ToSKColor( ), StrokeWidth = 1 } },
            { "MOON",            new SKPaint { Style = SKPaintStyle.StrokeAndFill, Color = Color.GhostWhite.ToSKColor( ), StrokeWidth = 1 } }
         };
         this.listView = false;

         this.size = new Dictionary<string, long>
         {
            { "PLANET",          10 },
            { "JUMP_GATE",       10 },
            { "GAS_GIANT",       10 },
            { "ORBITAL_STATION", 10 },
            { "ASTEROID_FIELD",  10 },
            { "MOON",            10 }
         };
         this.ListViewCommand = new Command( ( ) => this.ListView = !this.listView );
         this.LoadWaypointsCommand = new Command( async( ) => await ExecuteLoadWaypointsCommand( ) );
         this.WaypointTapped = new Command< WaypointViewModel >( OnWaypointSelected );
      }

      public Command ListViewCommand { get; }
      public Command LoadWaypointsCommand { get; }      
      public Command< WaypointViewModel > WaypointTapped { get; }

      public string Symbol
      {
         get
         {
            return( symbol );
         }
         set
         {
            LoadSystem( value );
            this.Title = value;
            OnPropertyChanged( "Title" );
            SetProperty( ref this.symbol, value );
            OnPropertyChanged( "Symbol" );
         }
      }

      public bool ListView
      {
         get
         {
            return ( this.listView );
         }
         set
         {
            SetProperty( ref this.listView, value );
            OnPropertyChanged( "ListView" );
         }
      }

      public ObservableCollection<WaypointViewModel> Waypoints { get => this.waypoints; }

      public void Draw( SKCanvas canvas )
      {
         canvas.Clear( );
         canvas.SetMatrix( matrix );

         SKRect clipBox;
         clipBox = new SKRect( 0, 0, ( float )canvas.LocalClipBounds.Width, ( float )canvas.LocalClipBounds.Height );
         clipBox = canvas.TotalMatrix.Invert( ).MapRect( clipBox );

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
         for( var i = 1; i < clipBox.Width; i += ( int )( clipBox.Width / 10 ) )
         {
            canvas.DrawCircle( clipBox.Width / 2, clipBox.Height / 2, i, gridPaint );
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
                  if( clipBox.Contains( waypoint.x, waypoint.y ) )
                  {
                     canvas.DrawCircle( waypoint.x, waypoint.y, size[ waypoint.type ], paint[ waypoint.type ] );
                     canvas.DrawText( waypoint.symbol, waypoint.x - 15, waypoint.y + 20, textPaint );
                     canvas.DrawText( string.Format( "({0},{1})", waypoint.x, waypoint.y ), waypoint.x - 15, waypoint.y + 40, textPaint );
                  }
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
               /*
               foreach( var system in this.VisibleSystems )
               {
                  if( rect.Contains( system.x, system.y ) )
                  {
                     Debug.WriteLine( string.Format( "Click system: {0}", system.symbol ) );
                     systemClicked = true;

                     break;
                  }
               }
               */

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

      public async void LoadSystem( string symbol )
      {
         this.system = await App.SystemDatabase.GetSystemAsync( symbol );
         this.system.waypoints = await navigation.GetSystemWaypoints( symbol );
         this.IsBusy = true;
      }

      public async Task ExecuteLoadWaypointsCommand( )
      {
         this.IsBusy = true;

         try
         {
            this.waypoints.Clear( );
            foreach( var waypoint in this.system.waypoints )
            {
               WaypointViewModel waypointVM = new WaypointViewModel( );
               await waypointVM.Initialize( waypoint.systemSymbol, waypoint.symbol );
               this.waypoints.Add( waypointVM );
            }
         }
         catch( Exception ex )
         {
            Debug.Write( ex );
         }
         finally
         {
            this.IsBusy = false;
         }
      }

      public async void OnWaypointSelected( WaypointViewModel waypoint )
      {
         if( waypoint != null )
         {
            // This will push the ItemDetailPage onto the navigation stack
            //await Shell.Current.GoToAsync( $"{nameof( ItemDetailPage )}?{nameof( ItemDetailViewModel.ItemId )}={item.Id}" );
         }
      }
   }
}
