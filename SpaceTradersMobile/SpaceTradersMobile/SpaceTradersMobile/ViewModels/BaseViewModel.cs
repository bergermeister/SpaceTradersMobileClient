namespace SpaceTradersMobile.ViewModels
{
   using System;
   using System.Collections.Generic;
   using System.ComponentModel;
   using System.Runtime.CompilerServices;
   using System.Text;

   public class BaseViewModel : INotifyPropertyChanged
   {
      private bool isBusy = false;
      private string title = string.Empty;

      public bool IsBusy
      {
         get { return( this.isBusy ); }
         set { SetProperty( ref this.isBusy, value ); }
      }

      public string Title
      {
         get { return( this.title ); }
         set { SetProperty( ref this.title, value ); }
      }

      protected bool SetProperty< T >( ref T backingStore, T value,
         [CallerMemberName] string propertyName = "",
         Action onChanged = null )
      {
         bool propertySet = false;

         if( EqualityComparer< T >.Default.Equals( backingStore, value ) )
         {
            propertySet = false;
         }
         else
         {
            backingStore = value;
            onChanged?.Invoke( );
            OnPropertyChanged( propertyName );
            propertySet = true;
         }

         return( propertySet );
      }

      #region INotifyPropertyChanged
      public event PropertyChangedEventHandler PropertyChanged;
      protected void OnPropertyChanged( [CallerMemberName] string propertyName = "" )
      {
         var changed = PropertyChanged;
         if( changed == null )
            return;

         changed.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
      }
      #endregion
   }
}
