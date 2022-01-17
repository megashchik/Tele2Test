using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DTO;
using PersonRepository;

namespace PersonsApi
{
    internal class WebInitializer
    {

        readonly string taskUri = "http://testlodtask20172.azurewebsites.net/task";
        readonly string taskIdUri = "http://testlodtask20172.azurewebsites.net/task/";

        public async Task<List<Person>> GetAllPeople()
        {
            using HttpClient client = new HttpClient();
            var response = await client.GetAsync(taskUri);
            string jsonPersons = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Person>>(jsonPersons) ?? new List<Person>();
        }

        public async Task<Person> GetPersonById(string id)
        {
            using HttpClient client = new HttpClient();
            var response = await client.GetAsync(taskIdUri + id);
            string jsonPerson = await response.Content.ReadAsStringAsync();
            Person? person = JsonSerializer.Deserialize<Person>(jsonPerson);
            if (person is object)
                return person with { Id = id };
            else
                throw new NullReferenceException("The value was null, possibly an invalid id was specified");
        }
    }
}
