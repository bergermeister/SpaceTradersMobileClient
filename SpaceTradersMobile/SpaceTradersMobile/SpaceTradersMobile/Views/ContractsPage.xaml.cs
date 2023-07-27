using SpaceTradersMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SpaceTradersMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ContractsPage : ContentPage
	{
		private ContractsViewModel contractsViewModel;

		public ContractsPage( )
		{
			InitializeComponent( );
			this.contractsViewModel = new ContractsViewModel( );
			this.BindingContext = this.contractsViewModel;
		}

      protected override void OnAppearing( )
      {
         base.OnAppearing( );
			this.contractsViewModel.IsBusy = true;
      }
   }
}