using Microsoft.AspNetCore.Mvc;

namespace vm22MVC.Models
{
    public class CollectiveModel
    {
        public CollectiveModel()
        {
            
        }
        public UserModel User { get; set; }
        public TournamentModel TournamentModel { get; set; }
        public List<TournamentModel> TournamentModelList { get; set; }

        //Constructor that takes two inputs and gives them values
        public CollectiveModel(UserModel user, TournamentModel tournamentModel, List<TournamentModel> tournamentModels)
        {
            User = user;
            TournamentModel = tournamentModel;
            TournamentModelList = tournamentModels;
        }

        public CollectiveModel(UserModel user, List<TournamentModel> tournamentModels)
        {
            User = user;
            TournamentModel = new TournamentModel();
            TournamentModelList = tournamentModels;
        }

        public CollectiveModel(UserModel user)
        {
            User = user;
            TournamentModel = new TournamentModel();
            TournamentModelList = new List<TournamentModel>();
        }
    }
}
