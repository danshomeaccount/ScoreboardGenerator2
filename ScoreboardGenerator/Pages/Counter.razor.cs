using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using ScoreboardGenerator.Models;
using System;
using System.Text;

namespace ScoreboardGenerator.Pages
{
    public partial class Counter
    {

        [Inject]
        public DialogService DialogService { get; set; } = default!;

        [Inject]        
        private NavigationManager NavManager { get; set; } = default!;

        private string _selectedPlayer = string.Empty;
        private string _playerOfTheMatch = string.Empty;


        public Match NewMatch { get; set; } = new Match();
        public List<Player> PlayerList = new() 
        { 
            new Player("James"),
            new Player("Jack"),
            new Player("Thomas"),
            new Player("Rory"),
            new Player("Xander"),
            new Player("Jude"),
            new Player("Oliver"),
            new Player("Seb"),
            new Player("Daniel")
        };

        protected RadzenDataGrid<Goal> ourScorersGrid = default!;
        protected RadzenDataGrid<Goal> opponentScorersGrid = default!;

        protected override void OnInitialized()
        {
            _selectedPlayer = PlayerList[0].Name;
            
            DialogService.OnClose += AddScoreDialogClosed;
        }

        public void AddScoreDialogClosed(dynamic addScore)
        {
            if (addScore)
            {
                var newGoal = new Goal();
                newGoal.Scored = DateTime.Now.Subtract(NewMatch.KickOff).TotalMinutes.ToString("0") + " minute(s)";
                newGoal.Scorer = SelectedPlayer;
                NewMatch.OurGoals.Add(newGoal);
                //reload dashbard
                ourScorersGrid.Reload();
                this.StateHasChanged();
            }
        }

        private async Task RemoveOurGoal()
        {
            int lastGoalIdx = NewMatch.OurGoals.Count - 1;
            if (lastGoalIdx >= 0)
                NewMatch.OurGoals.RemoveAt(lastGoalIdx);
            //reload dashbard
            await ourScorersGrid.Reload();
            this.StateHasChanged();

        }

        private async Task AddOpponentGoal()
        {
            var newGoal = new Goal();
            newGoal.Scored = DateTime.Now.Subtract(NewMatch.KickOff).TotalMinutes.ToString("0") + " minute(s)";
            newGoal.Scorer = "Unknown";
            NewMatch.OpponentGoals.Add(newGoal);
            //reload dashbard
            await opponentScorersGrid.Reload();
            this.StateHasChanged();
        }

        private async Task RemoveOpponentGoal()
        {
            int lastGoalIdx = NewMatch.OpponentGoals.Count - 1;
            if (lastGoalIdx >= 0)
                NewMatch.OpponentGoals.RemoveAt(lastGoalIdx);
            //reload dashbard
            await opponentScorersGrid.Reload();
            this.StateHasChanged();

        }

        public string SelectedPlayer
        {
            get => _selectedPlayer;
            set { _selectedPlayer = value; }
        }

        public string PlayerOfTheMatch
        {
            get => _playerOfTheMatch;
            set { _playerOfTheMatch = value; }
        }

        private async void HandleValidSubmit()
        {
            //const int feeEarnerId = 1;
            //await Service.CreateFeeEarnerCase(feeEarnerId, NewInstruction);
            //NavManager.NavigateTo("/mycases");
        }



        private void HandleInvalidSubmit()
        {

        }

        public class Match
        {
            public string OpponentName { get; set; } = string.Empty;
            public string Ground { get; set; } = string.Empty;
            public DateTime KickOff { get; set; } = DateTime.Now;
            public List<Goal> OurGoals { get; set; } = new List<Goal>();
            public List<Goal> OpponentGoals { get; set; } = new List<Goal>();
            public string PlayerOfTheMatch { get; set; } = string.Empty;

            public int OurScore { 
                get 
                {
                    return OurGoals.Count;                                   
                } 
            }

            public int OpponentScore {
                get {
                    return OpponentGoals.Count;
                }
            }
        }

        public class Player
        {

            public Player(string name)
            {
                Name = name;
            }
            public string Name { get; set; }

        }

    }
}
