namespace SpaceTradersMobile.ViewModels
{
   using SpaceTradersMobile.Models;
   using SpaceTradersMobile.Services;
   using Xamarin.Forms;

   public class DataStoreBaseViewModel : BaseViewModel
   {
      public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>( );
   }
}
