using SpaceTradersMobile.Models;
using SpaceTradersMobile.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SpaceTradersMobile.ViewModels
{
   public class ContractsViewModel : BaseViewModel
   {
      private Services.AgentAPI agentAPI;

      public ContractsViewModel( )
      {
         this.Title = "Contracts";
         this.agentAPI = DependencyService.Get<Services.AgentAPI>( );
         this.Contracts = new ObservableCollection< Models.Contract >( );
         this.LoadContractsCommand = new Command( async ( ) => await ExecuteLoadContracts( ) );
         this.ContractTapped = new Command< Models.Contract >( OnContractTapped );
      }

      public ObservableCollection< Models.Contract > Contracts { get; }
      public Command LoadContractsCommand { get; }
      public Command< Models.Contract > ContractTapped { get; }

      public async Task ExecuteLoadContracts( )
      {
         this.IsBusy = true;

         try
         {
            this.Contracts.Clear( );
            var contracts = await this.agentAPI.GetContracts( );
            foreach( var contract in contracts )
            {
               this.Contracts.Add( contract );
            }
         }
         catch( Exception ex )
         {
            Debug.WriteLine( ex );
         }
         finally
         {
            this.IsBusy = false;
         }
      }

      public async void OnContractTapped( Models.Contract contract )
      {
         if( contract != null )
         {
            await Shell.Current.GoToAsync( $"{nameof( ContractDetailsPage )}?{nameof( ContractDetailsViewModel.Id )}={contract.id}" );
         }
      }
   }
}
