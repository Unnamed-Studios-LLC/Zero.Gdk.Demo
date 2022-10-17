namespace Zero.Demo.Model.Api
{
    public class CreateConnectionRequest
    {
        public string Name { get; set; }
        public int Hat { get; set; }
        public int Head { get; set; }
        public int Legs { get; set; }
        public int Flag { get; set; }
        public int FlagColor { get; set; }
        public long RegionId { get; set; }
        public string WorldKey { get; set; }
        public string ClientVersion { get; set; }
    }
}
