using Contacts.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.Infrastructure.Extensions
{
    public static class PersonExtensions
    {
        public static PersonViewModel MapToViewModel(this Person person)
        {
            return new PersonViewModel(person.Id, person.Name, person.Title);
        }
    }
}
