﻿namespace SpaceTradersMobile.Views
{
   using Newtonsoft.Json;
   using System.Collections.ObjectModel;
   using System.Diagnostics;
   using System.Net.Http.Headers;
   using Xamarin.Forms;
   using Xamarin.Forms.Xaml;

   [XamlCompilation( XamlCompilationOptions.Compile )]
   public partial class AgentPage : ContentPage
   {
      private ViewModels.AgentViewModel accountViewModel;
      private Services.AgentAPI agentAPI = DependencyService.Get< Services.AgentAPI >( );
      private Services.Navigation waypointAPI = DependencyService.Get< Services.Navigation >( );
      private ObservableCollection< Models.Faction > factions;

      public AgentPage( )
      {
         InitializeComponent( );

         this.accountViewModel = new ViewModels.AgentViewModel( );
         this.factions = new ObservableCollection< Models.Faction >( );
         this.BindingContext = this.accountViewModel;
         this.FactionPicker.BindingContext = this;
         this.FactionPicker.ItemsSource = this.factions;
      }

      protected override void OnAppearing( )
      {
         getFactions( );
         if( Application.Current.Properties.ContainsKey( "agentToken" ) )
         {
            getAgent( );
            this.TokenEntry.Text = Application.Current.Properties[ "agentToken" ] as string;
         }
         getFactions( );
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

      private async void getFactions( )
      {
         var receivedFactions = await agentAPI.GetFactions( );
         if( receivedFactions == null )
         {
            await DisplayAlert( "ERROR", "Failed to receive Factions.", "Ok" );
         }
         else
         {
            this.Dispatcher.BeginInvokeOnMainThread( ( ) =>
            {
               this.factions.Clear( );
               foreach( var faction in receivedFactions )
               {
                  this.factions.Add( faction );
               }
            } );
         }
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
         httpService.AuthenticatedClient.DefaultRequestHeaders.Authorization =
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
         string startingFaction = ( this.FactionPicker.SelectedItem as Models.Faction ).symbol; //this.StartingFactionEntry.Text;
         this.registerAgent( symbol, startingFaction );
      }

      private void UpdateTokenClicked( object sender, System.EventArgs eventArgs )
      {
         string newToken = this.TokenEntry.Text;
         updateToken( newToken );
      }
   }
}