using DTO;

namespace PersonRepository
{
    public interface IPersonContainer
    {
        public Task AddPeople(IEnumerable<Person> people);
        public Task<Person> GetPersonById(string id);
        public Task<List<Person>> GetPeople(int count = 50, int skip = 0, Sex sex = Sex.Any, int minAge = int.MinValue, int maxAge = int.MaxValue);
    }
}
