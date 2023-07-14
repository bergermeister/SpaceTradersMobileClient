namespace SpaceTradersMobile.ViewModels
{
   using SpaceTradersMobile.Views;
   using System;
   using System.Collections.ObjectModel;
   using System.Diagnostics;
   using System.Threading.Tasks;
   using Xamarin.Forms;

   public class WaypointsViewModel : DataStoreBaseViewModel
   {
      private Models.Waypoint selectedWaypoint;
      private Services.Navigation waypointAPI;

      public ObservableCollection< Models.Waypoint > Waypoints { get; }
      public Command LoadItemsCommand { get; }
      public Command AddItemCommand { get; }
      public Command< Models.Waypoint > WaypointTapped { get; }
      
      public WaypointsViewModel( ) 
      {
         this.Title = "Waypoints";
         this.Waypoints = new ObservableCollection< Models.Waypoint >( );
         this.waypointAPI = DependencyService.Get< Services.Navigation >( );
         LoadItemsCommand = new Command( async ( ) => await ExecuteLoadWaypointsCommand( ) );
         WaypointTapped = new Command< Models.Waypoint >( OnWaypointSelected );
         //AddItemCommand = new Command( OnAddItem );
      }

      public async Task ExecuteLoadWaypointsCommand( )
      {
         IsBusy = true;

         try
         {
            /*
            Waypoints.Clear( );
            var waypoints = await DataStore.GetItemsAsync( true );
            foreach( var waypoint in waypoints )
            {
               Waypoints.Add( waypoint );
            }
            */
         }
         catch( Exception ex )
         {
            Debug.WriteLine( ex );
         }
         finally
         {
            IsBusy = false;
         }
      }

      public void OnAppearing( )
      {
         IsBusy = true;
         SelectedWaypoint = null;
      }

      public Models.Waypoint SelectedWaypoint
      {
         get => selectedWaypoint;
         set
         {
            SetProperty( ref selectedWaypoint, value );
            OnWaypointSelected( value );
         }
      }

      private async void OnAddItem( object obj )
      {
         //await Shell.Current.GoToAsync( nameof( NewItemPage ) );
      }

      async void OnWaypointSelected( Models.Waypoint waypoint )
      {
         if( waypoint != null )
         {
            // This will push the ItemDetailPage onto the navigation stack
            //await Shell.Current.GoToAsync( $"{nameof( ItemDetailPage )}?{nameof( ItemDetailViewModel.ItemId )}={item.Id}" );
         }
      }
   }
}
