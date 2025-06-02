using DesktopProgram.Data;
using DesktopProgram.Models;

namespace DesktopProgram.ViewModels
{
    public interface IBuildingService
    {
        List<Resource> GetAllResources();
        void SaveBuilding(Building building);
    }

    public class BuildingService : IBuildingService
    {
        private readonly ApplicationDbContext _context;

        public BuildingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Resource> GetAllResources()
        {
            return _context.Resources.OrderBy(r => r.Name).ToList();
        }

        public void SaveBuilding(Building building)
        {
            _context.Buildings.Add(building);
            _context.SaveChanges();
        }
    }
}
