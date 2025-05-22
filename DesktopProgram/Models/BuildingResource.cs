namespace DesktopProgram.Models
{
    public class BuildingResource
    {
        public int Id { get; set; }

        public int BuildingId { get; set; }
        public Building Building { get; set; }

        public int ResourceId { get; set; }
        public Resource Resource { get; set; }

        public int Quantity { get; set; }
    }
}