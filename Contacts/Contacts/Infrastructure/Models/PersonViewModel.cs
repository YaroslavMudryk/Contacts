namespace Contacts.Infrastructure.Models
{
    public class PersonViewModel
    {
        public PersonViewModel(int id, string name, string title)
        {
            Id = id;
            Name = $"{name} ({title})";
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}