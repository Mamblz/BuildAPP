using Xunit;
using Microsoft.EntityFrameworkCore;
using DesktopProgram.ViewModels;
using DesktopProgram.Models;
using DesktopProgram.Data;
using System.Linq;

public class ResourcesViewModelTests
{
    private ApplicationDbContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + System.Guid.NewGuid())
            .Options;
        return new ApplicationDbContext(options);
    }

    private ResourcesViewModel GetViewModel(ApplicationDbContext context = null)
    {
        if (context == null)
            context = GetInMemoryContext();
        return new ResourcesViewModel(context);
    }

    [Theory]
    [InlineData("", "100")]
    [InlineData("Name", "")]
    [InlineData("", "")]
    [InlineData("Name", "abc")]
    public void AddResource_InvalidInput_DoesNotAddResource(string name, string costText)
    {
        var context = GetInMemoryContext();
        var vm = GetViewModel(context);

        vm.Name = name;
        vm.CostText = costText;

        vm.AddResource();
        vm.LoadResources();

        Assert.Empty(vm.Resources);
    }

    [Fact]
    public void DeleteResource_ExistingResource_RemovesResource()
    {
        var context = GetInMemoryContext();
        var vm = GetViewModel(context);

        var resource = new Resource { Name = "ToDelete", Cost = 100 };
        context.Resources.Add(resource);
        context.SaveChanges();

        vm.LoadResources();

        vm.DeleteResource(resource);

        vm.LoadResources();

        Assert.Empty(vm.Resources);
    }

    [Fact]
    public void DeleteResource_NullResource_DoesNothing()
    {
        var context = GetInMemoryContext();
        var vm = GetViewModel(context);

        vm.DeleteResource(null);
        vm.LoadResources();
        Assert.Empty(vm.Resources);
    }

    [Fact]
    public void LoadResources_LoadsAllResourcesFromContext()
    {
        var context = GetInMemoryContext();

        context.Resources.Add(new Resource { Name = "R1", Cost = 10 });
        context.Resources.Add(new Resource { Name = "R2", Cost = 20 });
        context.SaveChanges();

        var vm = GetViewModel(context);

        vm.LoadResources();

        Assert.Equal(2, vm.Resources.Count);
        Assert.Contains(vm.Resources, r => r.Name == "R1" && r.Cost == 10);
        Assert.Contains(vm.Resources, r => r.Name == "R2" && r.Cost == 20);
    }

    [Fact]
    public void Properties_SettersRaisePropertyChanged()
    {
        var context = GetInMemoryContext();
        var vm = GetViewModel(context);

        bool nameChanged = false;
        bool costChanged = false;
        bool errorChanged = false;

        vm.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(vm.Name)) nameChanged = true;
            if (e.PropertyName == nameof(vm.CostText)) costChanged = true;
            if (e.PropertyName == nameof(vm.ErrorMessage)) errorChanged = true;
        };

        vm.Name = "TestName";
        vm.CostText = "123";
        vm.ErrorMessage = "Error";

        Assert.True(nameChanged);
        Assert.True(costChanged);
        Assert.True(errorChanged);
    }

    [Fact]
    public void AddResource_EmptyName_DoesNotAdd()
    {
        var context = GetInMemoryContext();
        var vm = GetViewModel(context);

        vm.Name = "   ";
        vm.CostText = "100";

        vm.AddResource();
        vm.LoadResources();

        Assert.Empty(vm.Resources);
    }

    [Fact]
    public void AddResource_InvalidCost_DoesNotAdd()
    {
        var context = GetInMemoryContext();
        var vm = GetViewModel(context);

        vm.Name = "ValidName";
        vm.CostText = "not_a_number";

        vm.AddResource();
        vm.LoadResources();

        Assert.Empty(vm.Resources);
    }

    [Fact]
    public void DeleteResource_ResourceNotInContext_DoesNotThrow()
    {
        var context = GetInMemoryContext();
        var vm = GetViewModel(context);

        var resource = new Resource { Id = 999, Name = "NonExisting", Cost = 123 };

        vm.DeleteResource(resource);

        vm.LoadResources();

        Assert.Empty(vm.Resources);
    }
}
