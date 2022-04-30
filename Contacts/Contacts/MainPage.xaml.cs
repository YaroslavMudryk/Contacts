using Contacts.Infrastructure.Extensions;
using Contacts.Infrastructure.Models;
using Contacts.Infrastructure.Services;
using Contacts.Infrastructure.Storages;
using System.Text;

namespace Contacts;

public partial class MainPage : ContentPage
{
    private List<PersonViewModel> _people;
    private List<Person> people;
    private readonly IStorage<Person> _storage;
    public MainPage()
    {
        InitializeComponent();
        _storage = new JsonFileStorage<Person>();
        people = _storage.GetAll();
        Sync();
    }

    public async void personsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        //var deviceInfo = GetDeviceInfo();
        //await DisplayAlert("Device info", deviceInfo, "Decline");

        var person = e.SelectedItem as PersonViewModel;
        await Navigation.PushAsync(new PersonView(_storage, PersonViewAction.View, person.Id));

        //await DisplayAlert("Людина", $"Користувач {person.Name} ({person.Id}) було знайдено", "Відміна", FlowDirection.LeftToRight);
    }

    private string GetDeviceInfo()
    {
        var sb = new StringBuilder();
        var device = DeviceInfo.Current;

        sb.AppendLine($"DeviceType: {device.DeviceType}");
        sb.AppendLine($"Idiom: {device.Idiom}");
        sb.AppendLine($"Manufacturer: {device.Manufacturer}");
        if (OperatingSystem.IsAndroid())
        {
            var deviceMapper = DeviceMapper.GetInstance();
            var trueModel = deviceMapper.GetDeviceByModel(device.Model);
            if (trueModel == null)
                sb.AppendLine($"Model: {device.Model}");
            else
                sb.AppendLine($"Model: {trueModel.MarketingName}");
        }
        else
        {
            sb.AppendLine($"Model: {device.Model}");
        }
        sb.AppendLine($"Name: {device.Name}");
        sb.AppendLine($"Platform: {device.Platform} {device.Version.Major}");

        return sb.ToString();
    }

    public async void btnAdd_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PersonView(_storage, PersonViewAction.Add, 0));
    }

    private async void caViewProfile_Clicked(object sender, EventArgs e)
    {
        var mItem = sender as MenuItem;
        var person = mItem.BindingContext as PersonViewModel;
        await Navigation.PushAsync(new PersonView(_storage, PersonViewAction.View, person.Id));
    }

    private async void caRemoveProfile_Clicked(object sender, EventArgs e)
    {
        var mItem = sender as MenuItem;
        var person = mItem.BindingContext as PersonViewModel;
        var res = await DisplayAlert("Система", $"Ви впевнені, що хочите видалити {person.Name}?", "Так", "Ні");
        if (res)
        {
            await DisplayAlert("Device info", "Успішно видалено", "Decline");
        }
    }

    private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (e.NewTextValue.Length == 0)
        {
            Sync();
        }
        if (e.NewTextValue.Length >= 2)
        {
            GlobalSearch(e.NewTextValue);
        }
    }

    private void GlobalSearch(string query)
    {
        var result = people
            .Where(s => s.Name.Contains(query)
            || (s.Title != null && s.Title.Contains(query))
            || (s.Emails != null && s.Emails.Contains(query))
            || (s.Phones != null && s.Phones.Contains(query))
            || (s.Hometown != null && s.Hometown.Contains(query))
            || (s.CityOfResidence != null && s.CityOfResidence.Contains(query))
            || (s.About != null && s.About.Contains(query))
            || (s.BandCards != null && s.BandCards.Contains(query))
            || (s.CarNumbers != null && s.CarNumbers.Contains(query))
            || (s.Study != null && s.Study.Contains(query))
            || (s.Work != null && s.Work.Contains(query)))
            .Select(s => s.MapToViewModel())
            .ToList();
        personsListView.ItemsSource = result;
    }

    private void Sync()
    {
        _people = people.Select(s => s.MapToViewModel()).ToList();
        personsListView.ItemsSource = _people;
    }
}