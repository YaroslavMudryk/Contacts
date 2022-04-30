using Contacts.Infrastructure.Models;
using Contacts.Infrastructure.Storages;

namespace Contacts;

public partial class PersonView : ContentPage
{
    private readonly IStorage<Person> _storage;
    private Person _person;
    private PersonViewAction _action;
    public PersonView(IStorage<Person> storage, PersonViewAction action, int id)
    {
        InitializeComponent();
        _storage = storage;
        _action = action;
        switch (action)
        {
            case PersonViewAction.Add: ConfigForAddPerson(); break;
            case PersonViewAction.Update: ConfigForUpdatePerson(id); break;
            case PersonViewAction.View: ConfigForViewPerson(id); break;
            default: break;
        }
    }

    private void ConfigForAddPerson()
    {
        actionButton.Text = "Додати";
        _person = new Person();
        SetEnabledFields(true);
        removeButton.IsVisible = false;
    }

    private void ConfigForUpdatePerson(int id)
    {
        actionButton.Text = "Зберегти";
        _person = _storage.GetById(id);
        MapModelToForm();
        SetEnabledFields(true);
    }

    private void ConfigForViewPerson(int id)
    {
        actionButton.Text = "Редагувати";
        _person = _storage.GetById(id);
        MapModelToForm();
        SetEnabledFields(false);
        removeButton.IsVisible = false;
    }

    private void MapModelToForm()
    {
        eId.Text = _person.Id == 0 ? ":)" : _person.Id.ToString();
        eName.Text = _person.Name;
        eTitle.Text = _person.Title;
        dpBirthday.Date = _person.Birthday ?? DateTime.Today;
        eEmails.Text = ArrayToString(_person.Emails);
        ePhones.Text = ArrayToString(_person.Phones);
        eHometown.Text = _person.Hometown;
        eCityOfResidence.Text = _person.CityOfResidence;
        eAbout.Text = _person.About;
        eBandCards.Text = ArrayToString(_person.BandCards);
        eCarNumbers.Text = ArrayToString(_person.CarNumbers);
        eStudy.Text = _person.Study;
        eWork.Text = _person.Work;
    }

    private void SetEnabledFields(bool status)
    {
        eId.IsEnabled = status;
        eName.IsEnabled = status;
        eTitle.IsEnabled = status;
        dpBirthday.IsEnabled = status;
        eEmails.IsEnabled = status;
        ePhones.IsEnabled = status;
        eHometown.IsEnabled = status;
        eCityOfResidence.IsEnabled = status;
        eAbout.IsEnabled = status;
        eBandCards.IsEnabled = status;
        eCarNumbers.IsEnabled = status;
        eStudy.IsEnabled = status;
        eWork.IsEnabled = status;
    }

    //private void SetReadOnlyFields(bool status)
    //{
    //    eName.IsReadOnly = status;
    //    eTitle.IsReadOnly = status;
    //    dpBirthday.IsEnabled = !status;
    //    eEmails.IsReadOnly = status;
    //    ePhones.IsReadOnly = status;
    //    eHometown.IsReadOnly = status;
    //    eCityOfResidence.IsReadOnly = status;
    //    eAbout.IsReadOnly = status;
    //    eBandCards.IsReadOnly = status;
    //    eCarNumbers.IsReadOnly = status;
    //    eStudy.IsReadOnly = status;
    //    eWork.IsReadOnly = status;
    //}

    private void MapFormToModel()
    {
        _person.Name = eName.Text;
        _person.Title = eTitle.Text;
        _person.Birthday = dpBirthday.Date;
        _person.Emails = StringToArray(eEmails.Text);
        _person.Phones = StringToArray(ePhones.Text);
        _person.Hometown = eHometown.Text;
        _person.CityOfResidence = eCityOfResidence.Text;
        _person.About = eAbout.Text;
        _person.BandCards = StringToArray(eBandCards.Text);
        _person.CarNumbers = StringToArray(eCarNumbers.Text);
        _person.Study = eStudy.Text;
        _person.Work = eWork.Text;
    }

    private void SavePerson()
    {
        MapFormToModel();
        if (_person.Id == 0)
        {
            _person = _storage.Create(_person);
        }
        else
        {
            _person = _storage.Update(_person);
        }
        _storage.Synchronize();
        MapModelToForm();
    }

    private async void actionButton_Clicked(object sender, EventArgs e)
    {
        var textButton = actionButton.Text;
        if (textButton == "Додати")
        {
            if (string.IsNullOrEmpty(eName.Text) || string.IsNullOrEmpty(eTitle.Text))
            {
                await DisplayAlert("Повідомлення", "Неможливо створити користувача без ім'я або нікнейму", "Ok");
                return;
            }
            SavePerson();
            SetEnabledFields(false);
            actionButton.Text = "Редагувати";
            removeButton.IsVisible = false;
        }
        if (textButton == "Редагувати")
        {
            SetEnabledFields(true);
            actionButton.Text = "Зберегти";
            removeButton.IsVisible = true;
        }
        if (textButton == "Зберегти")
        {
            SavePerson();
            SetEnabledFields(false);
            actionButton.Text = "Редагувати";
            removeButton.IsVisible = false;
        }
    }

    private string ArrayToString(string[] array)
    {
        if (array == null || array.Length == 0)
            return null;
        return string.Join(" ", array);
    }

    private string[] StringToArray(string content)
    {
        if (string.IsNullOrEmpty(content) || string.IsNullOrWhiteSpace(content))
            return null;
        return content.Split(" ");
    }

    private async void removeButton_Clicked(object sender, EventArgs e)
    {
        if (_person.Id != 0)
        {
            _storage.Remove(_person.Id);
            await Navigation.PopToRootAsync();
        }
    }
}

public enum PersonViewAction
{
    Add,
    Update,
    View
}