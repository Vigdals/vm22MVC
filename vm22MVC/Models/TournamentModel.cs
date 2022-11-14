using getAPIstuff.Models;

namespace vm22MVC.Models
{
    public class TournamentModel
    {
        public TournamentModel()
        {

        }
        public List<kampModel> kampModels { get; set; }
        public string groupName { get; set; }
        public int yearStart { get; set; }
        public int id { get; set; }
        public TippeModel TippeModel { get; set; }
    }
}
