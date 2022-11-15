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

        //Constructor that takes two inputs and gives them values
        public CollectiveModel(UserModel user, TournamentModel tournamentModel)
        {
            User = user;
            TournamentModel = tournamentModel;
        }
        public CollectiveModel(UserModel user)
        {
            User = user;
            TournamentModel = new TournamentModel();
        }
    }
}
