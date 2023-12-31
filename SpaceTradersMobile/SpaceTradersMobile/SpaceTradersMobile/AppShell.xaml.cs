﻿using SpaceTradersMobile.ViewModels;
using SpaceTradersMobile.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace SpaceTradersMobile
{
   public partial class AppShell : Xamarin.Forms.Shell
   {
      public AppShell( )
      {
         InitializeComponent( );
         Routing.RegisterRoute( nameof( ItemDetailPage ), typeof( ItemDetailPage ) );
         Routing.RegisterRoute( nameof( NewItemPage ), typeof( NewItemPage ) );
         Routing.RegisterRoute( nameof( SystemPage ), typeof( SystemPage ) );
         Routing.RegisterRoute( nameof( ContractDetailsPage ), typeof( ContractDetailsPage ) );
      }

   }
}
