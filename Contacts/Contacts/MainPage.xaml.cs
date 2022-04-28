using Contacts.Infrastructure.Extensions;
using Contacts.Infrastructure.Models;
using Contacts.Infrastructure.Services;
using Contacts.Infrastructure.Storages;
using System.Text;

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
        if (_people == null || _people.Count == 0)
            _people = new List<PersonViewModel>
            {
                new PersonViewModel(1, "efg", "reg")
            };
        personsListView.ItemsSource = _people;
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

    private void caViewProfile_Clicked(object sender, EventArgs e)
    {
        var person = new Person();
        Navigation.PushAsync(new PersonView(_storage, PersonViewAction.View, person.Id)).GetAwaiter().GetResult();
    }

    private void caRemoveProfile_Clicked(object sender, EventArgs e)
    {
        var res = DisplayAlert("Система", $"Ви впевнені, що хочите видалити User?", "Так", "Ні").GetAwaiter().GetResult();
        if (res)
        {

        }
    }
}