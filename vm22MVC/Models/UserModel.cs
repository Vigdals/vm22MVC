using Microsoft.AspNetCore.Mvc;

namespace vm22MVC.Models
{
    public class UserModel
    {
        public UserModel()
        {

        }
        public string BrukerNavn { get; set; }
        public string GruppeNavn { get; set; }
        public int Id { get; set; }
    }
}
