namespace Contacts.Infrastructure.Models
{
    public class Person : BaseModel
    {
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public string[] Email { get; set; }
        public string[] Phone { get; set; }
        public string Hometown { get; set; }
        public string CityOfResidence { get; set; }
        public string About { get; set; }
        public string[] BandCards { get; set; }
        public string[] CarNumbers { get; set; }
        public string Study { get; set; }
        public string Work { get; set; }
    }
}