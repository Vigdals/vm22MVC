namespace vm22MVC.Models
{
    public class StageStatsModel
    {
        public int StageId { get; set; }
        public string StageName { get; set; }
        public string FullName { get; set; }
        public string TournamentName { get; set; }
        public int YearStart { get; set; }
        public int YearEnd { get; set; }
        public int Matches { get; set; }
        public int MatchesStarted { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
        public int MinutesPlayed { get; set; }
        public int YellowCards { get; set; }
        public int RedCards { get; set; }

    }
}
