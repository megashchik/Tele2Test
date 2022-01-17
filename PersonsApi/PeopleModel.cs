using DTO;
using PersonRepository;
using System.Text.Json;

namespace PersonsApi
{
    public class PeopleModel
    {
        IPeopleContainer peopleContainer = new MySqlPeopleContainer();

        public PeopleModel()
        {
            new DbInitializer().InitDatabase(peopleContainer);
        }

        public int PageSize { get; set; } = 10;
        public async Task<List<Person>> GetPeople(int page = 0, Sex sex = Sex.Any, int minAge = int.MinValue, int maxAge = int.MaxValue)
        {
            return await peopleContainer.GetPeople(PageSize, page * PageSize, sex, minAge, maxAge);
        }
        public async Task<Person> GetPerson(string id)
        {
            return await peopleContainer.GetPersonById(id);
        }
    }
}