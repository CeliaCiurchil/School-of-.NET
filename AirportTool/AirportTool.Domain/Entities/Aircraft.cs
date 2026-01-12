namespace AirportTool.Domain.Entities
{
    public class Aircraft
    {
        public int Id { get; set; }
        public string TailNumber { get; set; } = null!;
        public string Model { get; set; } = null!;
        public int SeatCapacity { get; set; }
        public int? AirlineId { get; set; }
    }
}
