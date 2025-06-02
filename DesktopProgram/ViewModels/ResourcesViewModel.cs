using DesktopProgram.Models;
using DesktopProgram.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

public class ResourcesViewModel : INotifyPropertyChanged
{
    private readonly ApplicationDbContext _context;

    public ObservableCollection<Resource> Resources { get; private set; } = new ObservableCollection<Resource>();

    private string _name;
    public string Name
    {
        get => _name;
        set { _name = value; OnPropertyChanged(nameof(Name)); }
    }

    private string _costText;
    public string CostText
    {
        get => _costText;
        set { _costText = value; OnPropertyChanged(nameof(CostText)); }
    }

    private string _errorMessage;
    public string ErrorMessage
    {
        get => _errorMessage;
        set { _errorMessage = value; OnPropertyChanged(nameof(ErrorMessage)); }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public ICommand AddResourceCommand { get; }
    public ICommand DeleteResourceCommand { get; }

    public ResourcesViewModel(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        LoadResources();
    }

    public void LoadResources()
    {
        Resources.Clear();
        var resources = _context.Resources.AsNoTracking().ToList();
        foreach (var r in resources)
            Resources.Add(r);
    }


    public void AddResource()
    {
        if (string.IsNullOrWhiteSpace(Name))
            return;

        if (!decimal.TryParse(CostText, out decimal cost))
            return;

        var resource = new Resource
        {
            Name = Name,
            Cost = (int)Math.Round(cost)
        };

        _context.Resources.Add(resource);

        var result = _context.SaveChanges();

        Console.WriteLine($"SaveChanges result: {result}");

        LoadResources();
    }



    public void DeleteResource(Resource resource)
    {
        if (resource == null)
            return;

        var entity = _context.Resources.Find(resource.Id);
        if (entity == null)
            return; 

        _context.Resources.Remove(entity);
        _context.SaveChanges();

        LoadResources();
    }


    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
