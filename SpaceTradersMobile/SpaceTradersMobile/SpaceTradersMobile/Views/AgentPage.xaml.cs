namespace SpaceTradersMobile.Views
{
   using Newtonsoft.Json;
   using System.Diagnostics;
   using Xamarin.Forms;
   using Xamarin.Forms.Xaml;

   [XamlCompilation( XamlCompilationOptions.Compile )]
   public partial class AgentPage : ContentPage
   {
      private ViewModels.AgentViewModel accountViewModel;
      private Services.SpaceTraderAPI api = DependencyService.Get< Services.SpaceTraderAPI >( );

      public AgentPage( )
      {
         InitializeComponent( );

         this.accountViewModel = new ViewModels.AgentViewModel( );
         BindingContext = this.accountViewModel;
      }

      protected override void OnAppearing( )
      {
         getAccount( );
         getSystemWaypoints( );
         getAvailableShips( );
         //getWaypoint( );
         //getContracts( );
         //acceptContract( );
      }

      protected override void OnDisappearing( )
      {
         base.OnDisappearing( );
      }

      private async void getAccount( )
      {
         var agent = await api.GetAgent( );
         this.Dispatcher.BeginInvokeOnMainThread( ( ) =>
         {
            if( agent != null )
            {
               this.accountViewModel.Agent = agent;
               Debug.WriteLine( JsonConvert.SerializeObject( agent ) );
            }
            else
            {
               Debug.WriteLine( "Agent is null" );
            }
         } );         
      }

      private async void getWaypoint( )
      {
         var waypoint = await api.GetWaypoint( "X1-MP2", "X1-MP2-12220Z" );
         this.Dispatcher.BeginInvokeOnMainThread( ( ) =>
         {
            if( waypoint != null )
            {
               Debug.WriteLine( JsonConvert.SerializeObject( waypoint ) );
            }
            else
            {
               Debug.WriteLine( "Waypoint is null" );
            }
         } );
      }

      private async void getSystemWaypoints( )
      {
         var waypoints = await api.GetSystemWaypoints( "X1-MP2" );
         this.Dispatcher.BeginInvokeOnMainThread( ( ) =>
         {
            if( waypoints != null )
            {
               Debug.WriteLine( JsonConvert.SerializeObject( waypoints ) );
            }
            else
            {
               Debug.WriteLine( "Waypoints is null" );
            }
         } );
      }

      private async void getContracts( )
      {
         var contracts = await api.GetContracts( );
         this.Dispatcher.BeginInvokeOnMainThread( ( ) =>
         {
            if( contracts != null )
            {
               Debug.WriteLine( JsonConvert.SerializeObject( contracts ) );
            }
            else
            {
               Debug.WriteLine( "Contracts is null" );
            }
         } );
      }

      private async void acceptContract( )
      {
         var contract = await api.AcceptContract( "cljutox5p0v0as60cxcucpy45" );
         this.Dispatcher.BeginInvokeOnMainThread( ( ) =>
         {
            if( contract != null )
            {
               Debug.WriteLine( JsonConvert.SerializeObject( contract ) );
            }
            else
            {
               Debug.WriteLine( "Contract is null" );
            }
         } );
      }

      private async void getAvailableShips( )
      {
         var ships = await api.GetAvailableShips( "X1-MP2", "X1-MP2-91657X" );
         this.Dispatcher.BeginInvokeOnMainThread( ( ) =>
         {
            if( ships != null )
            {
               Debug.WriteLine( JsonConvert.SerializeObject( ships ) );
            }
            else
            {
               Debug.WriteLine( "Ships is null" );
            }
         } );
      }
   }
}