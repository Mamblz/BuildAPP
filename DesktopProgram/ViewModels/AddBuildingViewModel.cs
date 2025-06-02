using DesktopProgram.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace DesktopProgram.ViewModels
{
    public class AddBuildingViewModel : INotifyPropertyChanged
    {
        private readonly IBuildingService _service;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Resource> Resources { get; set; }
        public ObservableCollection<BuildingResource> SelectedResources { get; set; }

        public string BuildingName { get; set; }
        public string Status { get; set; }
        public string ProgressText { get; set; }
        public Resource SelectedResource { get; set; }
        public string ResourceQuantityText { get; set; }

        public Building CreatedBuilding { get; private set; }

        public AddBuildingViewModel(IBuildingService service)
        {
            _service = service;

            Resources = new ObservableCollection<Resource>(_service.GetAllResources());
            SelectedResources = new ObservableCollection<BuildingResource>();
        }

        public string AddResource()
        {
            if (SelectedResource == null)
                return "Выберите ресурс";

            if (!int.TryParse(ResourceQuantityText, out int quantity) || quantity <= 0)
                return "Введите корректное количество";

            var existing = SelectedResources.FirstOrDefault(r => r.ResourceId == SelectedResource.Id);
            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                SelectedResources.Add(new BuildingResource
                {
                    ResourceId = SelectedResource.Id,
                    Resource = SelectedResource,
                    Quantity = quantity
                });
            }

            ResourceQuantityText = "";
            return null;
        }

        public void RemoveResource(BuildingResource resource)
        {
            SelectedResources.Remove(resource);
        }

        public string ValidateAndCreateBuilding()
        {
            if (string.IsNullOrWhiteSpace(BuildingName))
                return "Введите название здания";

            if (string.IsNullOrWhiteSpace(Status))
                return "Введите статус здания";

            if (!int.TryParse(ProgressText, out int progress) || progress < 0 || progress > 100)
                return "Введите корректный прогресс (0-100)";

            var building = new Building
            {
                Name = BuildingName.Trim(),
                Status = Status.Trim(),
                Progress = progress,
                BuildingResources = SelectedResources
                    .Select(r => new BuildingResource
                    {
                        ResourceId = r.ResourceId,
                        Quantity = r.Quantity
                    }).ToList()
            };

            _service.SaveBuilding(building);
            CreatedBuilding = building;

            return null;
        }
    }
}
