using DTO;
using PersonRepository;
using System.Text.Json;

namespace PersonsApi
{
    public class PersonModel
    {
        readonly string taskUri = "http://testlodtask20172.azurewebsites.net/task";
        readonly string taskIdUri = "http://testlodtask20172.azurewebsites.net/task/";



        public PersonModel()
        {
            InitDatabase().GetAwaiter().GetResult();
        }

        async Task InitDatabase()
        {
            IList<Person> people = new List<Person>();
            IPersonContainer container = new MySqlPersonContainer();
            var peopleWithoutAge = await GetAllPeopleFromWeb();
            foreach (Person p in peopleWithoutAge)
            {
                people.Add((await GetPersonByIdFromWeb(p.Id)));
            }
            await container.AddPeople(people);
        }

        public int PageSize { get; set; } = 10;

        public async Task<List<Person>> GetPeople(int page = 0, Sex sex = Sex.Any, int minAge = int.MinValue, int maxAge = int.MaxValue)
        {
            IPersonContainer personContainer = new MySqlPersonContainer();
            return await personContainer.GetPeople(PageSize, page * PageSize, sex, minAge, maxAge);
        }
        public async Task<Person> GetPerson(string id)
        {
            IPersonContainer personContainer = new MySqlPersonContainer();
            return await personContainer.GetPersonById(id);
        }

        async Task<List<Person>> GetAllPeopleFromWeb()
        {
            using HttpClient client = new HttpClient();
            var response = await client.GetAsync(taskUri);
            string jsonPersons = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Person>>(jsonPersons) ?? new List<Person>();
            //return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Person>>(jsonPersons) ?? new List<Person>();
        }

        async Task<Person> GetPersonByIdFromWeb(string id)
        {
            using HttpClient client = new HttpClient();
            var response = await client.GetAsync(taskIdUri + id);
            string jsonPerson = await response.Content.ReadAsStringAsync();
            Person? person = JsonSerializer.Deserialize<Person>(jsonPerson);
            //Person? person = Newtonsoft.Json.JsonConvert.DeserializeObject<Person>(jsonPerson);
            if (person is object)
                return person with { Id = id };
            else
                throw new NullReferenceException("The value was null, possibly an invalid id was specified");
        }
    }
}