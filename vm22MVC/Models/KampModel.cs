﻿namespace getAPIstuff.Models
{
    public class kampModel
    {
        public kampModel()
        {

        }
        public int Id { get; set; }
        public int nifsKampId { get; set; }
        public string Name { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string HomeScore { get; set; }
        public string AwayScore { get; set; }
        public string HomeTeamLogo { get; set; }
        public string AwayTeamLogo { get; set; }
        public string Stadium { get; set; }
        public string Attendance { get; set; }
        public string TimeStamp { get; set; }
        public string GroupName { get; set; }
        //Setter NotPlayed som standardvariabel i enum'en
        public KampStatus KampStatus { get; set; } = KampStatus.NotPlayed;
    }

    public enum KampStatus
    {
        NotPlayed, Home, Tie, Away
    }
}
