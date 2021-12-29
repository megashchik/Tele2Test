using NUnit.Framework;
using PersonsApi;
using System.Linq;
using System.Threading.Tasks;
using DTO;

namespace PersonApiTest
{
    public class ApiTest
    {
        PersonModel api = new PersonModel();
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task GetPeopleOfPage()
        {
            var pageSize = 3;
            api.PageSize = pageSize;
            var people = await api.GetPeople();
            Assert.AreEqual(pageSize, people.Count());
            pageSize = 10;
            api.PageSize = pageSize;
            people = await api.GetPeople();
            Assert.AreEqual(pageSize, people.Count());
        }

        [Test]
        public async Task DefunctPage()
        {
            api.PageSize = 100;
            var emptyList = await api.GetPeople(page: 1);
            Assert.True(emptyList!.Count == 0);
        }

        [Test]
        public async Task GetPeopleBySex()
        {
            var males = await api.GetPeople(sex: Sex.Male);
            Assert.True(males.All(m => m.Sex == Sex.Male) && males.Count > 0);
            var women = await api.GetPeople(sex: Sex.Female);
            Assert.True(women.All(m => m.Sex == Sex.Female) && women.Count > 0);
        }

        [Test]
        public async Task GetPeopleByAge()
        {
            var young = await api.GetPeople(maxAge: 14);
            Assert.True(young.All(m => m.Age <= 14) && young.Count > 0);
            var superYoung = await api.GetPeople(maxAge: -14);
            Assert.True(superYoung.Count == 0);
            var elders = await api.GetPeople(minAge: 30);
            Assert.True(elders.All(m => m.Age >= 30) && elders.Count > 0);
            var middle = await api.GetPeople(minAge: 30, maxAge: 50);
            Assert.True(middle.All(m => m.Age >= 30 && m.Age <= 50) && middle.Count > 0);
        }

        [Test]
        public async Task GetPersonById()
        {
            string id = "guyqwhoij6";
            var person = await api.GetPerson(id);
            Assert.AreEqual(id, person.Id);
        }

        [Test]
        public async Task GetPersonByInvalidId()
        {
            try
            {
                await api.GetPerson("non-existent id");
            }
            catch (System.IO.InvalidDataException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }
}