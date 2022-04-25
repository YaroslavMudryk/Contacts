using Contacts.Infrastructure.Extensions;
using Contacts.Infrastructure.Models;
using Contacts.Infrastructure.Storages;

namespace Contacts;

public partial class MainPage : ContentPage
{
    private List<PersonViewModel> _people;
    private readonly IStorage<Person> _storage;
    public MainPage()
    {
        InitializeComponent();
        _storage = new JsonFileStorage<Person>();
        _people = _storage.GetAll(0, 50).Select(s => s.MapToViewModel()).ToList();
        personsListView.ItemsSource = _people;
    }

    private async void personsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        var person = e.SelectedItem as PersonViewModel;
        await DisplayAlert("Людина", $"Користувач {person.Name} ({person.Id}) було знайдено", "Відміна", FlowDirection.LeftToRight);
    }
}