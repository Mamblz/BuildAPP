using Xunit;
using Moq;
using System.Collections.Generic;
using DesktopProgram.Models;
using DesktopProgram.ViewModels;
using System.Linq;

public class AddBuildingViewModelTests
{
    private AddBuildingViewModel CreateViewModelWithResources()
    {
        var mockService = new Mock<IBuildingService>();
        mockService.Setup(s => s.GetAllResources()).Returns(new List<Resource>
        {
            new Resource { Id = 1, Name = "Дерево" },
            new Resource { Id = 2, Name = "Камень" }
        });

        mockService.Setup(s => s.SaveBuilding(It.IsAny<Building>()))
            .Verifiable();

        return new AddBuildingViewModel(mockService.Object);
    }


    [Fact]
    public void AddResource_ShouldAddNewResource()
    {
        var vm = CreateViewModelWithResources();
        vm.SelectedResource = vm.Resources.First();
        vm.ResourceQuantityText = "10";

        var error = vm.AddResource();

        Assert.Null(error);
        Assert.Single(vm.SelectedResources);
        Assert.Equal(10, vm.SelectedResources[0].Quantity);
        Assert.Equal("", vm.ResourceQuantityText);
    }


    [Fact]
    public void AddResource_ShouldUpdateExistingResource()
    {
        var vm = CreateViewModelWithResources();
        var resource = vm.Resources.First();

        vm.SelectedResources.Add(new BuildingResource
        {
            Resource = resource,
            ResourceId = resource.Id,
            Quantity = 5
        });

        vm.SelectedResource = resource;
        vm.ResourceQuantityText = "3";

        var error = vm.AddResource();

        Assert.Null(error);
        Assert.Single(vm.SelectedResources);
        Assert.Equal(8, vm.SelectedResources[0].Quantity);
    }


    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("abc")]
    [InlineData("-5")]
    [InlineData("0")]
    public void AddResource_ShouldFailWithInvalidQuantity(string input)
    {
        var vm = CreateViewModelWithResources();
        vm.SelectedResource = vm.Resources.First();
        vm.ResourceQuantityText = input;

        var error = vm.AddResource();

        Assert.NotNull(error);
        Assert.Empty(vm.SelectedResources);
    }


    [Fact]
    public void AddResource_ShouldFailIfNoResourceSelected()
    {
        var vm = CreateViewModelWithResources();
        vm.SelectedResource = null;
        vm.ResourceQuantityText = "10";

        var error = vm.AddResource();

        Assert.Equal("Выберите ресурс", error);
        Assert.Empty(vm.SelectedResources);
    }


    [Fact]
    public void RemoveResource_ShouldRemoveExistingResource()
    {
        var vm = CreateViewModelWithResources();
        var resource = vm.Resources.First();

        var br = new BuildingResource { Resource = resource, ResourceId = resource.Id, Quantity = 5 };
        vm.SelectedResources.Add(br);

        vm.RemoveResource(br);

        Assert.Empty(vm.SelectedResources);
    }


    [Fact]
    public void ValidateAndCreateBuilding_ShouldFailIfNameMissing()
    {
        var vm = CreateViewModelWithResources();
        vm.BuildingName = " ";
        vm.Status = "В процессе";
        vm.ProgressText = "50";

        var error = vm.ValidateAndCreateBuilding();

        Assert.Equal("Введите название здания", error);
    }


    [Fact]
    public void ValidateAndCreateBuilding_ShouldFailIfStatusMissing()
    {
        var vm = CreateViewModelWithResources();
        vm.BuildingName = "Дом";
        vm.Status = "";
        vm.ProgressText = "50";

        var error = vm.ValidateAndCreateBuilding();

        Assert.Equal("Введите статус здания", error);
    }


    [Fact]
    public void ValidateAndCreateBuilding_ShouldFailIfProgressNotNumber()
    {
        var vm = CreateViewModelWithResources();
        vm.BuildingName = "Дом";
        vm.Status = "Строится";
        vm.ProgressText = "abc";

        var error = vm.ValidateAndCreateBuilding();

        Assert.Equal("Введите корректный прогресс (0-100)", error);
    }


    [Theory]
    [InlineData("-1")]
    [InlineData("101")]
    public void ValidateAndCreateBuilding_ShouldFailIfProgressOutOfRange(string progress)
    {
        var vm = CreateViewModelWithResources();
        vm.BuildingName = "Дом";
        vm.Status = "Строится";
        vm.ProgressText = progress;

        var error = vm.ValidateAndCreateBuilding();

        Assert.Equal("Введите корректный прогресс (0-100)", error);
    }

    [Fact]
    public void ValidateAndCreateBuilding_ShouldCreateBuilding_WhenValid()
    {
        var vm = CreateViewModelWithResources();
        vm.BuildingName = "Форт";
        vm.Status = "Строится";
        vm.ProgressText = "75";

        vm.SelectedResources.Add(new BuildingResource
        {
            ResourceId = 1,
            Quantity = 10,
            Resource = vm.Resources.First()
        });

        var error = vm.ValidateAndCreateBuilding();

        Assert.Null(error);
        Assert.NotNull(vm.CreatedBuilding);
        Assert.Equal("Форт", vm.CreatedBuilding.Name);
        Assert.Equal("Строится", vm.CreatedBuilding.Status);
        Assert.Equal(75, vm.CreatedBuilding.Progress);
        Assert.Single(vm.CreatedBuilding.BuildingResources);
        Assert.Equal(10, vm.CreatedBuilding.BuildingResources.First().Quantity);
    }

    [Fact]
    public void ValidateAndCreateBuilding_ShouldCreateBuildingWithoutResources()
    {
        var vm = CreateViewModelWithResources();
        vm.BuildingName = "Хижина";
        vm.Status = "Планируется";
        vm.ProgressText = "0";

        var error = vm.ValidateAndCreateBuilding();

        Assert.Null(error);
        Assert.NotNull(vm.CreatedBuilding);
        Assert.Empty(vm.CreatedBuilding.BuildingResources);
    }
}
