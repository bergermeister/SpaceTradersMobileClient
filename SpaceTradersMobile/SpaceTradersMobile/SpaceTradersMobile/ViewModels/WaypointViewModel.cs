using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SpaceTradersMobile.ViewModels
{
   public class WaypointViewModel : BaseViewModel
   {
      private Models.Waypoint waypoint;
      private Services.Navigation navigationAPI;
      private ObservableCollection< Models.Trait > traits;

      public WaypointViewModel(  ) 
      {
         this.Title = "Unknown";
         this.waypoint = new Models.Waypoint( );
         this.navigationAPI = DependencyService.Get< Services.Navigation >( );
         this.traits = new ObservableCollection<Models.Trait>( );
         this.TraitTapped = new Command< Models.Trait >( OnTraitSelected );
      }

      public Command< Models.Trait > TraitTapped { get; }
      public string Symbol { get => this.waypoint.symbol; }
      public string Type { get => this.waypoint.type; }
      public ObservableCollection< Models.Trait > Traits { get => this.traits; }

      public async Task Initialize( string systemSymbol, string waypointSymbol )
      {
         this.Title = waypointSymbol;
         this.waypoint = await navigationAPI.GetWaypoint( systemSymbol, waypointSymbol );
         this.traits.Clear( );
         if( waypoint == null )
         {
            Debug.WriteLine( "Waypoint is null" );
         }
         else
         {
            foreach( var trait in waypoint.traits )
            {
               this.traits.Add( trait );
            }
         }
         OnPropertyChanged( "Title" );
      }

      async void OnTraitSelected( Models.Trait trait )
      {
         if( waypoint != null )
         {
            // This will push the ItemDetailPage onto the navigation stack
            //await Shell.Current.GoToAsync( $"{nameof( ItemDetailPage )}?{nameof( ItemDetailViewModel.ItemId )}={item.Id}" );
         }
      }
   }
}
