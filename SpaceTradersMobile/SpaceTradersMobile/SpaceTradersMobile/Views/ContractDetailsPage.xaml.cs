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
	public partial class ContractDetailsPage : ContentPage
	{
		private ContractDetailsViewModel contractViewModel;

		public ContractDetailsPage( )
		{
			InitializeComponent( );
			this.contractViewModel = new ContractDetailsViewModel( );
			this.BindingContext = this.contractViewModel;
		}
	}
}