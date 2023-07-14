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
   public partial class NavigationPage : ContentPage
   {
      private Services.Navigation navigation = DependencyService.Get< Services.Navigation >( );
      private List< Models.System > systems = null;

      public NavigationPage( )
      {
         InitializeComponent( );
         getSystems( );
      }

      private async void getSystems( )
      {
         systems = await navigation.GetSystems( );
      }

      private void CanvasViewPaintSurface( object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e )
      {

      }
   }
}