using getAPIstuff.Models;
using Microsoft.Build.Framework;

namespace vm22MVC.Models
{
    public class TippeModel
    {
        public TippeModel() { }

        public int Id { get; set; }
        [Required]
        public string Answer { get; set; }
        public string Gruppe { get; set; }
        public string HjemmeLag { get; set; }
        public string BorteLag { get; set; }
        public int nifsKampId { get; set; }
        public string userName { get; set; }
        public int Score { get; set; }

        public void UpdateScore(kampModel kamp)
        {
            var enumAsInt = (int)kamp.KampStatus;
            switch (kamp.KampStatus)
            {
                case KampStatus.NotPlayed:
                {
                    Score = 0;
                    break;
                }
                case KampStatus.Home:
                {
                    Score = Answer == "Home" ? 1 : 0;
                    break;
                }
                case KampStatus.Tie:
                {
                    Score = Answer == "Tie" ? 1 : 0;
                    break;
                }
                case KampStatus.Away:
                {
                    Score = Answer == "Away" ? 1 : 0;
                    break;
                }
                default: Score = 0;
                    break;
            }


            var resultat = kamp.KampStatus;
        }
    }
}
