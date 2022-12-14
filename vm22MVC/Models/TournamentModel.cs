using getAPIstuff.Models;

namespace vm22MVC.Models
{
    public class TournamentModel
    {
        public TournamentModel()
        {

        }
        public List<kampModel> kampModels { get; set; }
        public List<TippeModel> TippeModels { get; set; }
        public List<PlayerModel> PlayerModels { get; set; }
        public string groupName { get; set; }
        public int yearStart { get; set; }
        public int id { get; set; }
        public string year { get; set; }
        public string userName { get; set; }
        public int PoengSum { get; set; }
        public string ToppScorer { get; set; }
        public string Assist { get; set; }
        public string VmsBesteSpelar { get; set; }
        public int CalculateScoreByUserName(string username)
        {
            return TippeModels.Where(t => t.userName == username).Sum(s => s.Score);
        }
    }
}