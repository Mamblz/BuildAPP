namespace DesktopProgram.Models
{
    public class Building
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int Progress { get; set; }

        public ICollection<BuildingResource> BuildingResources { get; set; } = new List<BuildingResource>();
    }
}
