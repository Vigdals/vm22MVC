namespace vm22MVC.Models
{
    public class TippeModel
    {
        public TippeModel()
        {

        }
        public int Id { get; set; }
        public string Hjemme { get; set; }
        public string Uavgjort { get; set; }
        public string Borte { get; set; }
        public string Gruppe { get; set; }
        public string HjemmeLag { get; set; }
        public string BorteLag { get; set; }
    }
}
