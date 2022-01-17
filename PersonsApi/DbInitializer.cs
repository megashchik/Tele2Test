using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using PersonRepository;

namespace PersonsApi
{
    internal class DbInitializer
    {
        public void InitDatabase(IPeopleContainer container)
        {
            InitDatabaseAsync(container).Wait();
        }

        async Task InitDatabaseAsync(IPeopleContainer container)
        {
            WebInitializer initializer = new WebInitializer();
            IList<Person> people = new List<Person>();
            var peopleWithoutAge = await initializer.GetAllPeople();
            foreach (Person p in peopleWithoutAge)
            {
                people.Add((await initializer.GetPersonById(p.Id)));
            }
            await container.AddPeople(people);
        }
    }
}
