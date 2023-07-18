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
      private Services.AgentAPI agentAPI;
      private Services.Navigation navigation;
      private ViewModels.UniverseViewModel universeVM;
      private ViewModels.SystemViewModel systemVM;
      private SKMatrix matrix;
      private Dictionary< long, SKPoint > touchDictionary;

      public NavigationPage( )
      {
         InitializeComponent( );

         this.agentAPI = DependencyService.Get< Services.AgentAPI >( );
         this.navigation = DependencyService.Get< Services.Navigation >( );
         this.universeVM = new ViewModels.UniverseViewModel( );
         this.systemVM = new ViewModels.SystemViewModel(  );
         this.UniverseCoordinateEntry.BindingContext = this.universeVM;
         this.matrix = SKMatrix.CreateIdentity( );
         this.touchDictionary = new Dictionary< long, SKPoint >( );
      }

      protected override void OnAppearing( )
      {
         /// @todo select universeVM or systemVM
         this.universeVM.ReloadSystems( );
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
         canvas.SetMatrix( matrix );
         /// @todo Switch between universeVM and systemVM draw methods
         this.universeVM.Draw( info, surface, canvas );      
         canvas.Flush( );
      }

      private void CanvasViewTouch( object sender, SKTouchEventArgs args )
      {
         //TouchTrackingPoint point = args.Location;
         SKPoint point = args.Location;

         //switch( args.Type )
         switch( args.ActionType )
         {
            //case TouchActionType.Pressed:
            case SKTouchAction.Pressed:
            {
               // Find transformed canvasview rectangle
               //SKRect rect = new SKRect( 0, 0, ( float )CanvasView.Width, ( float )CanvasView.Height );
               SKRect rect = new SKRect( point.X - 10, point.Y - 10, point.X + 10, point.Y + 10 );
               rect = matrix.Invert( ).MapRect( rect );

               bool systemClicked = false;
               /*
               foreach( var system in this.universeVM.VisibleSystems )
               {
                  if( rect.Contains( system.x, system.y ) )
                  {
                     systemClicked = true;
                     break;
                  }
               }
               */
               // Determine if the touch was within that rectangle
               if( !systemClicked && !touchDictionary.ContainsKey( args.Id ) ) // && rect.Contains( skPoint ) )
               {
                  touchDictionary.Add( args.Id, point );
               }
               break;
            }
            //case TouchActionType.Moved:
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
                     CanvasView.InvalidateSurface( );
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
                        matrix.PostConcat( scaleMatrix ); //SKMatrix.PostConcat( ref matrix, scaleMatrix );
                        CanvasView.InvalidateSurface( );
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