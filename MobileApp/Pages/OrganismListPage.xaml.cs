using System.Collections.ObjectModel;
using MobileApp.Models;
using MobileApp.Services;

namespace MobileApp.Pages;

public partial class OrganismListPage : ContentPage
{
    private readonly OrganismApiService _organismService;

    public ObservableCollection<Organism> Organisms { get; } = new();

    public OrganismListPage(OrganismApiService service)
    {
        InitializeComponent();
        _organismService = service;
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var list = await _organismService.GetAllAsync();
        Organisms.Clear();
        foreach (var o in list) Organisms.Add(o);
    }
}
