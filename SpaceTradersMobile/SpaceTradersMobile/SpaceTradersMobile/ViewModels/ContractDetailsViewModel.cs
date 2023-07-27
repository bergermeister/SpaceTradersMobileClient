using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SpaceTradersMobile.ViewModels
{
   [QueryProperty( nameof( Id ), nameof( Id ) ) ]
   public class ContractDetailsViewModel : BaseViewModel
   {
      private string id;
      private Models.Contract contract;
      private Services.AgentAPI agentAPI;

      public ContractDetailsViewModel( )
      {
         this.agentAPI = DependencyService.Get< Services.AgentAPI >( );
         this.Deliveries = new ObservableCollection<Models.Delivery>( );
         this.AcceptContractCommand = new Command( async( ) => await AcceptContract( id ) );
      }

      public string Id
      {
         get
         {
            return( this.id );
         }
         set
         {
            this.LoadContract( value );
            this.Title = value;
            OnPropertyChanged( "Title" );
            SetProperty( ref this.id, value );
            OnPropertyChanged( "Id" );
         }
      }

      public Models.Contract Contract { get => this.contract; }

      public ObservableCollection< Models.Delivery > Deliveries { get; }

      public Command AcceptContractCommand { get; }

      public async void LoadContract( string contractId )
      {
         this.contract = await agentAPI.GetContract( contractId );
         this.Deliveries.Clear( );
         foreach( var delivery in this.contract.terms.deliver )
         {
            this.Deliveries.Add( delivery );
         }
         //OnPropertyChanged( "Contract" );
         OnPropertyChanged( "id" );
         OnPropertyChanged( "factionSymbol" );
         OnPropertyChanged( "type" );
         OnPropertyChanged( "accepted" );
         OnPropertyChanged( "fulfilled" );
         OnPropertyChanged( "expiration" );
         OnPropertyChanged( "deadlineToAccept" );
      }

      public async Task AcceptContract( string contractId )
      {
         this.contract = await agentAPI.AcceptContract( contractId );
         this.LoadContract( this.id );
      }
   }
}
