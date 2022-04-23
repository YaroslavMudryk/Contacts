namespace Contacts.Infrastructure.Models
{
    public class Group : BaseModel
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public int[] PersonIds { get; set; }
    }
}