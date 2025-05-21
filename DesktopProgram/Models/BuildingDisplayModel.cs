using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopProgram.Models
{
    public class BuildingDisplayModel
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public int Progress { get; set; }
        public List<ResourceDisplayModel> BuildingResources { get; set; }
    }

    public class ResourceDisplayModel
    {
        public string ResourceInfo { get; set; }
    }

}