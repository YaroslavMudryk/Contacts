namespace Contacts.Infrastructure.Models
{
    public class Person : BaseModel
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public DateTime? Birthday { get; set; }
        public string[] Emails { get; set; }
        public string[] Phones { get; set; }
        public string Hometown { get; set; }
        public string CityOfResidence { get; set; }
        public string About { get; set; }
        public string[] BandCards { get; set; }
        public string[] CarNumbers { get; set; }
        public string Study { get; set; }
        public string Work { get; set; }
    }
}