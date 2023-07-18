namespace SpaceTradersMobile.ViewModels
{
   internal class AgentViewModel : BaseViewModel
   {
      private Models.Agent agent;

      public AgentViewModel( )
      {
         this.Title = "Unknown";
         this.agent = new Models.Agent( );
         this.agent.accountId = "-";
         this.agent.symbol = "-";
         this.agent.credits = 0;
         this.agent.headquarters = "-";
         this.agent.startingFaction = "-";
   }

      public Models.Agent Agent
      {
         get => this.agent;
         set
         {
            this.Title = this.agent.symbol;
            SetProperty( ref this.agent, value );
            OnPropertyChanged( "AccountId" );
            OnPropertyChanged( "Symbol" );
            OnPropertyChanged( "Headquarters" );
            OnPropertyChanged( "Credits" );
            OnPropertyChanged( "StartingFaction" );
         }
      }

      public string AccountId { get => this.agent.accountId; }
      public string Symbol { get => this.agent.symbol; }
      public string Headquarters { get => this.agent.headquarters; }
      public long Credits { get => this.agent.credits; }
      public string StartingFaction { get => this.agent.startingFaction; }
   }
}
