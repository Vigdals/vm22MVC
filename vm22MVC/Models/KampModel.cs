using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace getAPIstuff.Models
{
    public class kampModel
    {
        public kampModel()
        {

        }
        public string Name { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string HomeScore { get; set; }
        public string AwayScore { get; set; }
        public string HomeTeamLogo { get; set; }
        public string AwayTeamLogo { get; set; }
        public string Stadium { get; set; }
        public string Attendance { get; set; }
        public string Timestamp { get; set; }
    }
}
