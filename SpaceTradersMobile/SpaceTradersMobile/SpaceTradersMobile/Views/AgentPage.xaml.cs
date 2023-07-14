namespace SpaceTradersMobile.Views
{
   using Newtonsoft.Json;
   using System.Collections.Generic;
   using System.Diagnostics;
   using System.Net.Http.Headers;
   using Xamarin.Forms;
   using Xamarin.Forms.Xaml;

   [XamlCompilation( XamlCompilationOptions.Compile )]
   public partial class AgentPage : ContentPage
   {
      private ViewModels.AgentViewModel accountViewModel;
      private Services.AgentAPI agentAPI = DependencyService.Get< Services.AgentAPI >( );
      private Services.ContractAPI contractAPI = DependencyService.Get< Services.ContractAPI >( );
      private Services.Navigation waypointAPI = DependencyService.Get< Services.Navigation >( );

      public AgentPage( )
      {
         InitializeComponent( );

         this.accountViewModel = new ViewModels.AgentViewModel( );
         BindingContext = this.accountViewModel;
      }

      protected override void OnAppearing( )
      {
         if( Application.Current.Properties.ContainsKey( "agentToken" ) )
         {
            getAgent( );
            this.TokenEntry.Text = Application.Current.Properties[ "agentToken" ] as string;
         }
      }

      protected override void OnDisappearing( )
      {
         base.OnDisappearing( );
      }

      private async void getAgent( )
      {
         var agent = await agentAPI.GetCurrent( );
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

      private async void registerAgent( string symbol, string startingFaction )
      {
         var agentToken = await agentAPI.Register( symbol, startingFaction );
         if( agentToken.Value == null )
         {
            await DisplayAlert( "ERROR", "Agent Registration Failed.", "Ok" );
         }
         else
         {
            this.Dispatcher.BeginInvokeOnMainThread( ( ) =>
            {
               this.accountViewModel.Agent = agentToken.Value;
               Debug.WriteLine( JsonConvert.SerializeObject( agentToken.Value ) );
            } );
         }
      }

      private async void updateToken( string token )
      {
         Services.HttpService httpService = DependencyService.Get< Services.HttpService >( );
         httpService.Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue( "Bearer", token );
         Application.Current.Properties[ "agentToken" ] = token;
         await Application.Current.SavePropertiesAsync( );
         getAgent( );
      }

      private async void getWaypoint( )
      {
         var waypoint = await waypointAPI.GetWaypoint( "X1-MP2", "X1-MP2-12220Z" );
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
         var waypoints = await waypointAPI.GetSystemWaypoints( "X1-MP2" );
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
         var contracts = await contractAPI.GetCurrent( );
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
         var contract = await contractAPI.AcceptContract( "cljutox5p0v0as60cxcucpy45" );
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
         var ships = await waypointAPI.GetAvailableShips( "X1-MP2", "X1-MP2-91657X" );
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

      private void RegisterNewAgentClicked( object sender, System.EventArgs eventArgs )
      {
         string symbol = this.SymbolEntry.Text;
         string startingFaction = this.StartingFactionEntry.Text;
         this.registerAgent( symbol, startingFaction );
      }

      private void UpdateTokenClicked( object sender, System.EventArgs eventArgs )
      {
         string newToken = this.TokenEntry.Text;

      }
   }
}