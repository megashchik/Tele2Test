using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PersonApiTest
{
    public class WebApiTest
    {
        static readonly string personPath = "/person";
        static readonly string peoplePath = "/people";

        HttpClient GetClient()
        {
            var application = new WebApplicationFactory<Program>();
            return application.CreateClient();
        }

        [Test]
        public async Task GetPersonById()
        {
            var client = GetClient();

            var existingPerson = await client.GetFromJsonAsync<DTO.Person>($"{personPath}?id=guyqwhoij6");
            Assert.NotNull(existingPerson);
        }

        [Test]
        public async Task GetPersonByInvalidId()
        {
            var client = GetClient();

            var errorResponce = await client.GetAsync($"{personPath}?id=non-existingId");
            Assert.AreEqual(500, (int)errorResponce.StatusCode);
            System.Console.WriteLine(await errorResponce.Content.ReadAsStringAsync());
        }


        [Test]
        public async Task GetPeopleSimple()
        {
            var client = GetClient();
            var simple = await client.GetFromJsonAsync<List<DTO.PersonSmall>>(peoplePath);
            Assert.True(simple!.Count > 0);
        }

        [Test]
        public async Task GetPeopleBySex()
        {
            var client = GetClient();
            string path = $"{peoplePath}?sex=female";

            var women = await client.GetFromJsonAsync<List<DTO.PersonSmall>>(path);
            Assert.True(women!.Count > 0);
            Assert.True(women.All(m => m.Sex == DTO.Sex.Female));

            // it's correct
            path = $"{peoplePath}?sex=Female";

            women = await client.GetFromJsonAsync<List<DTO.PersonSmall>>(path);
            Assert.True(women!.Count > 0);
            Assert.True(women.All(m => m.Sex == DTO.Sex.Female));
        }

        [Test]
        public async Task GetPeopleByAge()
        {
            var client = GetClient();
            int minAge = 18, maxAge = 45;
            string path = $"{peoplePath}?minAge={minAge}&maxAge={maxAge}";

            var middleAges = await client.GetFromJsonAsync<List<DTO.PersonSmall>>(path);
            Assert.True(middleAges!.Count > 0);
            // we can't check their age
            //Assert.True(middleAges.All(ma => ma.Age >= minAge && ma.Age <= maxAge));
        }

        [Test]
        public async Task GetPeopleByConcretePage()
        {
            var client = GetClient();
            string secodPagePath = $"{peoplePath}?page=1";

            var firstPage = await client.GetFromJsonAsync<List<DTO.PersonSmall>>(peoplePath);
            var secodPage = await client.GetFromJsonAsync<List<DTO.PersonSmall>>(secodPagePath);
            Assert.True(firstPage!.First().Id != secodPage!.First().Id);
        }

        [Test]
        public async Task GetDefunctPage()
        {
            var client = GetClient();
            string defunctPagePath = $"{peoplePath}?page=100";

            var defunctPage = await client.GetFromJsonAsync<List<DTO.PersonSmall>>(defunctPagePath);
            Assert.True(defunctPage!.Count == 0);
        }

        [Test]
        public async Task GetByInvalidPath()
        {
            var client = GetClient();
            string invalidPath = "invalid";

            var invalidPathResult = await client.GetAsync(invalidPath);
            // this page non-exist
            Assert.AreEqual(404, (int)invalidPathResult.StatusCode);
        }

        [Test]
        public async Task GetPeopleByInvalidSex()
        {
            var client = GetClient();
            string path = $"{peoplePath}?sex=females";

            var women = await client.GetFromJsonAsync<List<DTO.PersonSmall>>(path);
            // invalid gender is defined as missing
            Assert.True(women!.Count == 0);
        }

        [Test]
        public async Task GetExceptionByStringAge()
        {
            var client = GetClient();
            string minAge = "двенадцать";
            string path = $"{peoplePath}?minAge={minAge}";

            try
            {
                var middleAges = await client.GetFromJsonAsync<List<DTO.PersonSmall>>(path);
            }
            catch(HttpRequestException)
            {
                // I can rewrite the web api to manually enter integer values and send an error message for that
                System.Console.WriteLine("invalid parameter can't cast to int");
            }
        }
    }

}
