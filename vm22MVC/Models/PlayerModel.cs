namespace vm22MVC.Models
{
    public class PlayerModel
    {
        public int nifsId { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string BirthDate { get; set; }
        //Stage = turnering eller gruppe. Ein stage = f.eks. gruppe C i vm2022
        public List<StageStatsModel> StageStatsModels { get; set; }

    }
}