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
    }
}
