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
        _person = new Person();
        //ToDo add init other ui elements
    }

    private void ConfigForUpdatePerson(int id)
    {
        _person = _storage.GetById(id);
        MapModelToForm();
    }

    private void ConfigForViewPerson(int id)
    {
        _person = _storage.GetById(id);
        MapModelToForm();
    }

    private void MapModelToForm()
    {

    }
}

public enum PersonViewAction
{
    Add,
    Update,
    View
}