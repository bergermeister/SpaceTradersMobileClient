using SkiaSharp.Views.Forms;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpaceTradersMobile.Views
{
   [XamlCompilation( XamlCompilationOptions.Compile )]
   public partial class SystemPage : ContentPage
   {
      private ViewModels.SystemViewModel systemVM;

      public SystemPage( )
      {
         InitializeComponent( );
         this.systemVM = new ViewModels.SystemViewModel( );
         this.BindingContext = this.systemVM;
      }

      protected override void OnAppearing( )
      {
         this.CanvasView.InvalidateSurface( );
      }

      private void CanvasViewPaintSurface( object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs eventArgs )
      {
         //SKImageInfo info = eventArgs.Info;
         SKSurface surface = eventArgs.Surface;
         SKCanvas canvas = surface.Canvas;
         this.systemVM.Draw( canvas );
      }

      private void CanvasViewTouch( object sender, SKTouchEventArgs args )
      {
         this.systemVM.Touch( sender as SKCanvasView, args );
      }
   }
}